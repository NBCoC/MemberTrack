using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MemberTrack.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MemberTrack.WebApi.Security
{
    public static class TokenProviderExtensions
    {
        private const string SecretKey = "_buffer_9#00!#8423-12834)*@$920*membertrack";
        private static SecurityKey SecurityKey { get; } = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public static IApplicationBuilder UseTokenProvider(this IApplicationBuilder app, string audience)
        {
            if (string.IsNullOrEmpty(audience)) throw new ArgumentNullException(nameof(audience));

            if (app == null) throw new ArgumentNullException(nameof(IApplicationBuilder));

            var options = GetTokenProviderOptions(audience);

            if (options == null) throw new ArgumentNullException(nameof(TokenProviderOptions));

            var parameters = GetTokenValidationParameters(options.Issuer, audience);

            return
                app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options)).UseJwtBearerAuthentication(
                    GetJwtBearerOptions(parameters));
        }

        private static JwtBearerOptions GetJwtBearerOptions(TokenValidationParameters parameters)
            =>
            new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = false,
                TokenValidationParameters = parameters
            };

        private static TokenValidationParameters GetTokenValidationParameters(string issuer, string audience)
            =>
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

        private static TokenProviderOptions GetTokenProviderOptions(string audience)
            =>
            new TokenProviderOptions
            {
                Audience = audience,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };

        private static async Task<ClaimsIdentity> GetIdentity(string email, string password, IUserService service)
            =>
            await service.Authenticate(email, password)
                ? new ClaimsIdentity(new GenericIdentity(email, "Token"), new Claim[] {}) : null;
    }
}