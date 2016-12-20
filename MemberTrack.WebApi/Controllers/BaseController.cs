using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using MemberTrack.Common;
using MemberTrack.Data;
using MemberTrack.Services.Exceptions;
using MemberTrack.WebApi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemberTrack.WebApi.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected BaseController(DatabaseContext context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        protected DatabaseContext Context { get; }

        protected ILogger Logger { get; }

        protected string ContextUserEmail
        {
            get
            {
                var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(CustomClaimTypes.Email));

                if (claim != null)
                {
                    return claim.Value;
                }

                Logger.LogError($"Failed to retrieve '{JwtRegisteredClaimNames.Sub}' claim type.");

                return null;
            }
        }

        protected IActionResult Exception(Exception e)
        {
            var message = e.ToDetail();

            Logger.LogError(message);

            var error = new ErrorResponse(message);

            if (e is BadRequestException || e is DuplicateEntityException || e is DbUpdateException)
            {
                return StatusCode(409, error);
            }

            if (e is UnauthorizeException)
            {
                return Unauthorized();
            }

            if (e is EntityNotFoundException)
            {
                return NotFound(error);
            }

            return BadRequest(e);
        }
    }
}