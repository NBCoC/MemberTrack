namespace MemberTrack.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Entities;
    using Dtos;
    using Exceptions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class PersonService : IPersonService
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public PersonService(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

        public async Task<IDbContextTransaction> BeginTransactionAsync(
                CancellationToken cancellationToken = new CancellationToken())
            => await _context.Database.BeginTransactionAsync(cancellationToken);

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

            var entity =
                await
                    _context.People.Include(p => p.Address).
                        Include(p => p.CheckLists).
                        ThenInclude(cl => cl.PersonCheckListItem).
                        FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            var dto = entity.ToDto();

            var existingItems = dto.CheckListItems.ToList();

            var checkListItems =
                await
                    _context.PersonCheckListItems.Where(pcli => existingItems.All(cli => cli.Id != pcli.Id)).
                        Select(
                            v =>
                                new PersonCheckListItemDto
                                {
                                    Description = v.Description,
                                    Id = v.Id,
                                    Type = v.CheckListItemType
                                }).
                        ToListAsync();

            existingItems.AddRange(checkListItems);

            dto.CheckListItems = existingItems;

            return dto;
        }

        public async Task<SearchResultDto<PersonSearchDto>> Search(string contains)
        {
            var count = await _context.People.CountAsync();

            if (string.IsNullOrEmpty(contains))
            {
                return new SearchResultDto<PersonSearchDto>(new List<PersonSearchDto>(), count);
            }

            contains = contains.ToLower();

            var query =
                await
                    _context.People.Where(
                            x => x.FirstName.ToLower().Contains(contains) || x.LastName.ToLower().Contains(contains)).
                        Take(25).
                        OrderBy(x => x.FirstName).
                        ThenBy(x => x.LastName).
                        ToListAsync();

            return new SearchResultDto<PersonSearchDto>(query.ToDtos(), count);
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

        public async Task UpdateDates(string contextUserEmail, DatesDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(DatesDto));
            }

            var entity = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);

            if (entity == null)
            {
                throw new EntityNotFoundException(personId);
            }

            entity.BaptismDate = dto.BaptismDate;
            entity.FirstVisitDate = dto.FirstVisitDate;
            entity.MembershipDate = dto.MembershipDate;

            await _context.SaveChangesAsync();
        }

        public async Task InsertChildrenInfo(string contextUserEmail, ChildrenInfoDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(ChildrenInfoDto));
            }

            var entity = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);

            if (entity == null)
            {
                throw new EntityNotFoundException(personId);
            }

            entity.HasElementaryKids = dto.HasElementaryKids;
            entity.HasHighSchoolKids = dto.HasHighSchoolKids;
            entity.HasInfantKids = dto.HasInfantKids;
            entity.HasJuniorHighKids = dto.HasJuniorHighKids;
            entity.HasToddlerKids = dto.HasToddlerKids;

            await _context.SaveChangesAsync();
        }

        public async Task InsertOrRemoveCheckListItem(
            string contextUserEmail, PersonCheckListItemDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(PersonCheckListItemDto));
            }

            var model =
                await
                    _context.PersonCheckLists.FirstOrDefaultAsync(
                        pcl => pcl.PersonCheckListItemId == dto.Id && pcl.PersonId == personId);

            if (model == null)
            {
                model = new PersonCheckList
                {
                    PersonId = personId,
                    PersonCheckListItemId = dto.Id,
                    Note = dto.Note,
                    Date = DateTimeOffset.UtcNow
                };

                _context.PersonCheckLists.Add(model);
            }
            else
            {
                _context.PersonCheckLists.Remove(model);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonCheckListItemLookupDto>> GetCheckListItemLookup()
            => (await _context.PersonCheckListItems.ToListAsync()).ToDtos();

        public async Task<PersonReportDto> GetReport()
        {
            var dto = new PersonReportDto();

            var date = DateTimeOffset.UtcNow;

            var lastYear = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero).AddYears(-1);

            var memberGroups =
                await
                    _context.People.Where(
                            x =>
                                (x.MembershipDate != null) && (x.MembershipDate <= date && x.MembershipDate >= lastYear) &&
                                x.Status == PersonStatusEnum.Member).
                        Select(x => new {x.MembershipDate}).
                        GroupBy(x => new {x.MembershipDate.Value.Year, x.MembershipDate.Value.Month}).
                        ToListAsync();

            foreach (var group in memberGroups)
            {
                var item = dto.Items.FirstOrDefault(x => x.Month == group.Key.Month);

                if (item == null) continue;

                if (group.Key.Month == date.Month &&
                    group.Key.Year < date.Year) continue;

                item.MemberCount += group.Count();
            }

            var visitorGroups =
                await
                    _context.People.Where(
                            x =>
                                (x.FirstVisitDate != null) && x.FirstVisitDate <= date && x.FirstVisitDate >= lastYear).
                        Select(x => new {x.FirstVisitDate}).
                        GroupBy(x => new {x.FirstVisitDate.Value.Year, x.FirstVisitDate.Value.Month}).
                        ToListAsync();

            foreach (var group in visitorGroups)
            {
                var item = dto.Items.FirstOrDefault(x => x.Month == group.Key.Month);

                if (item == null) continue;

                if (group.Key.Month == date.Month &&
                    group.Key.Year < date.Year) continue;

                item.VisitorCount += group.Count();
            }

            return dto;
        }
    }
}