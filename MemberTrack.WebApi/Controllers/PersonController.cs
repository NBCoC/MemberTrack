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
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonController(DatabaseContext context, ILoggerFactory loggerFactory, IPersonService personService)
            : base(context, loggerFactory.CreateLogger<PersonController>())
        {
            _personService = personService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _personService.Find(x => x.Id == id);

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _personService.Delete(ContextUserEmail, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonInsertOrUpdateDto dto)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                var id = await _personService.Insert(ContextUserEmail, dto);

                trans.Commit();

                return Ok(id);
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PersonInsertOrUpdateDto dto)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _personService.Update(ContextUserEmail, dto, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [Route("[action]")]
        [HttpPost("{id}")]
        public async Task<IActionResult> ChildrenInfo(long id, [FromBody] ChildrenInfoDto dto)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _personService.InsertChildrenInfo(ContextUserEmail, dto, id);

                trans.Commit();

                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();

                return Exception(e);
            }
        }

        [Route("[action]")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Visit(long id, [FromBody] VisitDto dto)
        {
            var trans = await Context.Database.BeginTransactionAsync();

            try
            {
                await _personService.InsertOrUpdateVisit(ContextUserEmail, dto, id);

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