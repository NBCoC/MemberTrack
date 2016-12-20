using MemberTrack.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MemberTrack.WebApi.Security
{
    public static class ExceptionHandlerExtensions
    {
        private static ILogger Logger { get; set; }

        public static IApplicationBuilder UseCustomExceptionHandler(
            this IApplicationBuilder builder, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(nameof(ExceptionHandlerExtensions));

            builder.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    if (error?.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await
                            context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse("Token expired"),
                                settings));
                    }
                    else if (error?.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        Logger.LogError(error.Error.ToDetail());

                        await
                            context.Response.WriteAsync(
                                JsonConvert.SerializeObject(new ErrorResponse(error.Error.Message), settings));
                    }

                    else await next();
                });
            });

            return builder;
        }
    }
}