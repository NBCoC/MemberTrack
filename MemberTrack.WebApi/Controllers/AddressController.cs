using System;
using System.Threading.Tasks;
using MemberTrack.Data;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MemberTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(DatabaseContext context, ILoggerFactory loggerFactory, IAddressService addressService)
            : base(context, loggerFactory.CreateLogger<AddressController>())
        {
            _addressService = addressService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _addressService.Delete(ContextUserEmail, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(long id, [FromBody] AddressInsertOrUpdateDto dto)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _addressService.InsertOrUpdate(ContextUserEmail, dto, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }
    }
}