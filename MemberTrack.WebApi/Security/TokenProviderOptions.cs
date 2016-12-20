using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MemberTrack.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace MemberTrack.WebApi.Security
{
    public class TokenProviderOptions
    {
        public string Path => "/api/token";

        public string Issuer => "MemberTrack";

        public string Audience { get; set; }

        public TimeSpan Expiration => TimeSpan.FromMinutes(25);

        public SigningCredentials SigningCredentials { get; set; }

        public Func<string, string, IUserService, Task<ClaimsIdentity>> IdentityResolver { get; set; }

        public Func<Task<string>> NonceGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
    }
}