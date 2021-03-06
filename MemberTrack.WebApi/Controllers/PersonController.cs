﻿namespace MemberTrack.WebApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Contracts;
    using Services.Dtos;

    [Route("api/[controller]")]
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonController(ILoggerFactory loggerFactory, IPersonService personService)
            : base(loggerFactory.CreateLogger<PersonController>())
        {
            _personService = personService;
        }

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true)]
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
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Delete(long id)
        {
            var trans = await _personService.BeginTransactionAsync();

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
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Post([FromBody] PersonInsertOrUpdateDto dto)
        {
            var trans = await _personService.BeginTransactionAsync();

            try
            {
                var id = await _personService.Insert(ContextUserEmail, dto);

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

        [HttpPut("{id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Put(long id, [FromBody] PersonInsertOrUpdateDto dto)
        {
            var trans = await _personService.BeginTransactionAsync();

            try
            {
                await _personService.Update(ContextUserEmail, dto, id);

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

        [Route("[action]")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Search(string contains)
        {
            try
            {
                var data = await _personService.Search(contains);

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [Route("[action]")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Report()
        {
            try
            {
                var data = await _personService.GetReport();

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [Route("[action]")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> RecentActivity()
        {
            try
            {
                var data = await _personService.GetRecentActivity();

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }
        
        [HttpPost("CheckListItem/{id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> CheckListItem(long id, [FromBody] PersonCheckListItemDto dto)
        {
            var trans = await _personService.BeginTransactionAsync();

            try
            {
                await _personService.InsertOrRemoveCheckListItem(ContextUserEmail, dto, id);

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