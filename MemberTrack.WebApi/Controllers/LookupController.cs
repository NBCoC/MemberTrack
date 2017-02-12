using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MemberTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : BaseController
    {
        private readonly IPersonService _personService;

        public LookupController(ILoggerFactory loggerFactory, IPersonService personService)
            : base(loggerFactory.CreateLogger<LookupController>())
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data =
                    new
                    {
                        Roles =
                        new List<ItemLookupDto>
                        {
                            new ItemLookupDto(UserRoleEnum.Viewer),
                            new ItemLookupDto(UserRoleEnum.Editor),
                            new ItemLookupDto(UserRoleEnum.Admin)
                        },
                        States = new List<ItemLookupDto> {new ItemLookupDto(StateEnum.TX)},
                        PersonStatus =
                        new List<ItemLookupDto>
                        {
                            new ItemLookupDto(PersonStatusEnum.Visitor),
                            new ItemLookupDto(PersonStatusEnum.Member)
                        },
                        Genders =
                        new List<ItemLookupDto>
                        {
                            new ItemLookupDto(GenderEnum.Male),
                            new ItemLookupDto(GenderEnum.Female)
                        },
                        CheckListItemTypes =
                        new List<ItemLookupDto>
                        {
                            new ItemLookupDto(CheckListItemTypeEnum.Unknown),
                            new ItemLookupDto(CheckListItemTypeEnum.FirstVisit),
                            new ItemLookupDto(CheckListItemTypeEnum.SecondVisit),
                            new ItemLookupDto(CheckListItemTypeEnum.ThirdVisit),
                            new ItemLookupDto(CheckListItemTypeEnum.FourthVisit),
                            new ItemLookupDto(CheckListItemTypeEnum.MembershipRequestDate),
                            new ItemLookupDto(CheckListItemTypeEnum.MembershipAnnouncementDate),
                            new ItemLookupDto(CheckListItemTypeEnum.LifeGroup),
                            new ItemLookupDto(CheckListItemTypeEnum.Ministry)
                        },
                        AgeGroups =
                        new List<ItemLookupDto>
                        {
                            new ItemLookupDto(AgeGroupEnum.Unknown),
                            new ItemLookupDto(AgeGroupEnum.Group1),
                            new ItemLookupDto(AgeGroupEnum.Group2),
                            new ItemLookupDto(AgeGroupEnum.Group3),
                            new ItemLookupDto(AgeGroupEnum.Group4),
                            new ItemLookupDto(AgeGroupEnum.Group5)
                        },
                        CheckListItems = await _personService.GetCheckListItemLookup()
                    };

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }
    }
}