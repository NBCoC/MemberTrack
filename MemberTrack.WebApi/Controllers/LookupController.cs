using System.Collections.Generic;
using MemberTrack.Common;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MemberTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : BaseController
    {
        public LookupController(DatabaseContext context, ILoggerFactory loggerFactory)
            : base(context, loggerFactory.CreateLogger<LookupController>())
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data =
                new
                {
                    Roles =
                    new List<KeyValuePair<UserRoleEnum, string>>
                    {
                        new KeyValuePair<UserRoleEnum, string>(UserRoleEnum.Viewer, UserRoleEnum.Viewer.ToDescription()),
                        new KeyValuePair<UserRoleEnum, string>(UserRoleEnum.Editor, UserRoleEnum.Editor.ToDescription()),
                        new KeyValuePair<UserRoleEnum, string>(UserRoleEnum.Admin, UserRoleEnum.Admin.ToDescription())
                    }
                };

            return Ok(data);
        }
    }
}