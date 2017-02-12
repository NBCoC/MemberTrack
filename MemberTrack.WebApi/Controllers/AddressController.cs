using System;
using System.Threading.Tasks;
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
        private readonly IPersonService _personService;

        public AddressController(ILoggerFactory loggerFactory, IAddressService addressService, IPersonService personService)
            : base(loggerFactory.CreateLogger<AddressController>())
        {
            _addressService = addressService;
            _personService = personService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var trans = await _addressService.BeginTransactionAsync();

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
            var trans = await _addressService.BeginTransactionAsync();

            try
            {
                await _addressService.InsertOrUpdate(ContextUserEmail, dto, id);

                trans.Commit();

                var data = await _personService.Find(x => x.Id == id);

                return Ok(data);
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }
    }
}