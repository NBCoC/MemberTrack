using System.Collections.Generic;
using System.Linq;
using MemberTrack.Common;
using MemberTrack.Data.Entities;
using MemberTrack.Data;

namespace MemberTrack.Services.Dtos
{
    using Common.Quiz;
    using Data.Entities.Quizzes;
    using System;

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
                Dates = new DatesDto
                {
                    BaptismDate = entity.BaptismDate,
                    FirstVisitDate = entity.FirstVisitDate,
                    MembershipDate = entity.MembershipDate
                },
                ChildrenInfo =
                    new ChildrenInfoDto
                    {
                        HasElementaryKids = entity.HasElementaryKids,
                        HasHighSchoolKids = entity.HasHighSchoolKids,
                        HasInfantKids = entity.HasInfantKids,
                        HasJuniorHighKids = entity.HasJuniorHighKids,
                        HasToddlerKids = entity.HasToddlerKids
                    },
                CheckListItems =
                    entity.CheckLists?.Select(
                        v =>
                            new PersonCheckListItemDto
                            {
                                Note = v.Note,
                                Date = v.Date,
                                Description = v.PersonCheckListItem?.Description,
                                Id = v.PersonCheckListItemId,
                                Type = v.PersonCheckListItem?.CheckListItemType ?? CheckListItemTypeEnum.Unknown,
                                SortOrder = v.PersonCheckListItem?.SortOrder ?? 0
                            })
            };

        public static IEnumerable<PersonSearchDto> ToDtos(this IEnumerable<Person> entities)
            =>
            entities.Select(
                entity =>
                    new PersonSearchDto
                    {
                        Id = entity.Id,
                        AgeGroup = entity.AgeGroup,
                        FirstName = entity.FirstName,
                        MiddleName = entity.MiddleName,
                        LastName = entity.LastName,
                        Status = entity.Status
                    });

        public static PersonCheckListItemLookupDto ToDto(this PersonCheckListItem entity)
            =>
            new PersonCheckListItemLookupDto
            {
                Id = entity.Id,
                Type = entity.CheckListItemType,
                Description = entity.Description,
                SortOrder = entity.SortOrder
            };

        public static IEnumerable<PersonCheckListItemLookupDto> ToDtos(this IEnumerable<PersonCheckListItem> entities)
            => entities.Select(ToDto);

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
                Status = dto.Status,
                CreatedDate = DateTimeOffset.UtcNow
            };

        public static UserDto ToDto(this User entity)
            => new UserDto {Id = entity.Id, DisplayName = entity.DisplayName, Email = entity.Email, Role = entity.Role};

        public static User ToEntity(this UserInsertDto dto)
            =>
            new User
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                Role = dto.Role,
                Password = SystemAccountHelper.DefaultPassword
            };

        public static IEnumerable<UserDto> ToDtos(this IEnumerable<User> entities) => entities.Select(ToDto);

        public static QuizDto ToDto(this Quiz entity)
            => new QuizDto { Id = entity.Id, Name = entity.Name, Description = entity.Description, QuestionCount = entity.Questions.Count, Instructions = entity.Instructions };

        public static QuizTerseDto ToTerseDto(this Quiz entity)
            => new QuizTerseDto { Id = entity.Id, Name = entity.Name, Description = entity.Description, QuestionCount = entity.Questions.Count };


        public static IEnumerable<QuizTerseDto> ToTerseDtos(this IEnumerable<Quiz> entities) => entities.Select(ToTerseDto);

        public static QuizAnswerDto ToDto(this Answer entity, int index, IQuizManager quizManager)
            => new QuizAnswerDto { Id = entity.Id, Name = quizManager.InterpolateAnswerName(entity, index), Description = entity.Description };

        //There is an assumption that the answers on the question instance are already sorted for display to the user when NextQuestion was called.
        public static QuizQuestionDto ToDto(this Question entity, IQuizManager quizManager)
            => new QuizQuestionDto
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        AllowMultipleAnswers = entity.AllowMultipleAnswers,
                        Answers = entity.Answers.Select((a, index) => a.ToDto(index, quizManager)),
                    };

    }
}