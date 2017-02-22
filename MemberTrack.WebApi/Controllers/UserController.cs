using System;
using System.Threading.Tasks;
using MemberTrack.Common;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MemberTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(ILoggerFactory loggerFactory, IUserService userService)
            : base(loggerFactory.CreateLogger<UserController>())
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _userService.Find(x => x.Id == id && x.Id != SystemAccountHelper.UserId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _userService.Where(x => x.Id != SystemAccountHelper.UserId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInsertDto dto)
        {
            var trans = await _userService.BeginTransactionAsync();

            try
            {
                var id = await _userService.Insert(ContextUserEmail, dto);

                trans.Commit();

                var data = await _userService.Find(x => x.Id == id);

                return Ok(data);
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UserUpdateDto dto)
        {
            var trans = await _userService.BeginTransactionAsync();

            try
            {
                await _userService.Update(ContextUserEmail, dto, id);

                trans.Commit();

                var data = await _userService.Find(x => x.Id == id);

                return Ok(data);
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var trans = await _userService.BeginTransactionAsync();

            try
            {
                await _userService.Delete(ContextUserEmail, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(long id, [FromBody] UserUpdatePasswordDto dto)
        {
            var trans = await _userService.BeginTransactionAsync();

            try
            {
                await _userService.UpdatePassword(ContextUserEmail, dto, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPost("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(long id)
        {
            var trans = await _userService.BeginTransactionAsync();

            try
            {
                await _userService.ResetPassword(ContextUserEmail, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ContextUser()
        {
            try
            {
                var data = await _userService.Find(x => x.Email.Equals(ContextUserEmail));

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult IsAuthenticated()
        {
            // If token hasnt been issued or is expired, consumer wont be able to access this method...
            return Ok();
        }
    }
}