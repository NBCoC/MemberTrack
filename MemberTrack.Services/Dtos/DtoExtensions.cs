using System.Collections.Generic;
using System.Linq;
using MemberTrack.Common;
using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public static class DtoExtensions
    {
        public static PersonDto ToDto(this Person entity)
            =>
            new PersonDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Email = entity.Email,
                ContactNumber = entity.ContactNumber,
                Gender = entity.Gender,
                AgeGroup = entity.AgeGroup,
                Status = entity.Status,
                Address = entity.Address?.ToDto(),
                ChildrenInfo = new ChildrenInfoDto {AgeGroups = entity.ChildrenInfos?.Select(ci => ci.AgeGroup)},
                Visits =
                    entity.Visits?.Select(
                        v =>
                            new VisitDto
                            {
                                Note = v.Note,
                                Date = v.Date,
                                CheckListItems = v.CheckList?.Select(cl => new VisitCheckListItemDto
                                {
                                    Description = cl.VisitCheckListItem?.Description,
                                    Id = cl.VisitCheckListItemId,
                                    Group = cl.VisitCheckListItem?.Group ?? 0
                                })
                            })
            };

        public static AddressDto ToDto(this Address entity)
            =>
            new AddressDto {Street = entity.Street, City = entity.City, ZipCode = entity.ZipCode, State = entity.State};

        public static Address ToEntity(this AddressInsertOrUpdateDto dto)
            => new Address {Street = dto.Street, City = dto.City, ZipCode = dto.ZipCode, State = dto.State};

        public static Person ToEntity(this PersonInsertOrUpdateDto dto)
            =>
            new Person
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Email = dto.Email,
                ContactNumber = dto.ContactNumber,
                Gender = dto.Gender,
                AgeGroup = dto.AgeGroup ?? AgeGroupEnum.Unknown,
                Status = PersonStatusEnum.Visitor
            };

        public static UserDto ToDto(this User entity)
            => new UserDto {Id = entity.Id, DisplayName = entity.DisplayName, Email = entity.Email, Role = entity.Role};

        public static User ToEntity(this UserInsertDto dto)
            => new User {DisplayName = dto.DisplayName, Email = dto.Email, Role = dto.Role, Password = SystemAccountHelper.DefaultPassword };

        public static IEnumerable<UserDto> ToDtos(this IEnumerable<User> entities) => entities.Select(ToDto);
    }
}