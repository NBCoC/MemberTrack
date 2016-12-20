using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Services
{
    public class PersonService : IPersonService
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public PersonService(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Delete(string contextUserEmail, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            var entity = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);

            if (entity == null)
            {
                throw new EntityNotFoundException(personId);
            }

            _context.People.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<PersonDto> Find(Expression<Func<Person, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var entity = await _context.People.FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            return entity.ToDto();
        }

        public async Task<long> Insert(string contextUserEmail, PersonInsertOrUpdateDto dto)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(PersonInsertOrUpdateDto));
            }

            var entity = dto.ToEntity();

            _context.People.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(string contextUserEmail, PersonInsertOrUpdateDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(PersonInsertOrUpdateDto));
            }

            var entity = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);

            if (entity == null)
            {
                throw new EntityNotFoundException(personId);
            }

            entity.FirstName = dto.FirstName;
            entity.MiddleName = dto.MiddleName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.ContactNumber = dto.ContactNumber;
            entity.Gender = dto.Gender;
            entity.Status = dto.Status;
            entity.AgeGroup = dto.AgeGroup ?? AgeGroupEnum.Unknown;

            await _context.SaveChangesAsync();
        }

        public async Task InsertChildrenInfo(string contextUserEmail, ChildrenInfoDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(ChildrenInfoDto));
            }

            var childrenInfo = await _context.ChildrenInfos.Where(ci => ci.PersonId == personId).ToListAsync();

            childrenInfo.ForEach(item => _context.ChildrenInfos.Remove(item));

            await _context.SaveChangesAsync();

            foreach (var ageGroup in dto.AgeGroups)
            {
                _context.ChildrenInfos.Add(new ChildrenInfo {PersonId = personId, AgeGroup = ageGroup});
            }

            await _context.SaveChangesAsync();
        }

        public async Task InsertOrUpdateVisit(string contextUserEmail, VisitDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(VisitDto));
            }

            var visit = await _context.Visits.FirstOrDefaultAsync(v => v.Date == dto.Date && v.VisitorId == personId);

            var checkList =
                dto.CheckListItems.Select(
                    cli => new VisitCheckList {VisitorId = personId, VisitCheckListItemId = cli.Id}).ToList();

            if (visit == null)
            {
                visit = new Visit {Note = dto.Note, Date = dto.Date, VisitorId = personId, CheckList = checkList};

                _context.Visits.Add(visit);
            }
            else
            {
                visit.Note = dto.Note;

                checkList.ForEach(item => visit.CheckList.Add(item));
            }

            await _context.SaveChangesAsync();
        }
    }
}