using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using MemberTrack.Common;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MemberTrack.WebApi.Security
{
    public class TokenProviderMiddleware
    {
        private const string TokenType = "Bearer";
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IUserService _userService;

        public TokenProviderMiddleware(
            RequestDelegate next, IOptions<TokenProviderOptions> options, ILoggerFactory loggerFactory,
            IServiceProvider services)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<TokenProviderMiddleware>();
            _userService = services.GetService(typeof(IUserService)) as IUserService;
            _serializerSettings = new JsonSerializerSettings {Formatting = Formatting.Indented};

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            ThrowIfInvalidOptions(_options);
        }

        public Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToString();

            if (!path.Contains(_options.Path))
            {
                return _next(context);
            }

            if (!context.Request.Method.Equals("POST"))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                return
                    context.Response.WriteAsync(
                        JsonConvert.SerializeObject(
                            new ErrorResponse(
                                "Bad token request. POST method and 'application/json' content-type is required."),
                            _jsonSerializerSettings));
            }

            _logger.LogInformation("Handling token request: " + context.Request.Path);

            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            var data = await GetBodyContent(context);

            if (_userService == null)
            {
                throw new BadRequestException($"Cannot resolve dependency of {nameof(IUserService)}.");
            }

            var identity = await _options.IdentityResolver(data.Email, data.Password, _userService);

            if (identity == null)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                await
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse("Invalid credentials."),
                        _jsonSerializerSettings));

                return;
            }

            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, data.Email), new Claim(CustomClaimTypes.Email, data.Email),
                new Claim(JwtRegisteredClaimNames.Jti, await _options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, now, now.Add(_options.Expiration),
                _options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            context.Response.ContentType = "application/json";

            var response =
                new
                {
                    access_token = encodedJwt,
                    expires_in = (int) _options.Expiration.TotalSeconds,
                    token_type = TokenType
                };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        private static long ToUnixEpochDate(DateTime date)
            =>
            (long)
            Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<LoginUser> GetBodyContent(HttpContext context)
        {
            var body = string.Empty;

            try
            {
                using (var stream = new MemoryStream())
                {
                    await context.Request.Body.CopyToAsync(stream);

                    stream.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(stream))
                    {
                        body = await reader.ReadToEndAsync();
                    }
                }

                return JsonConvert.DeserializeObject<LoginUser>(body);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Body content: {body}{Environment.NewLine}{e.ToDetail()}");

                throw new BadRequestException(
                    "Failed to deserialize credentials. Content-Type of application/json is requried.");
            }
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.Path)) throw new ArgumentNullException(nameof(TokenProviderOptions.Path));

            if (string.IsNullOrEmpty(options.Issuer)) throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience)) throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero) throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));

            if (options.IdentityResolver == null) throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));

            if (options.SigningCredentials == null) throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));

            if (options.NonceGenerator == null) throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
        }
    }
}