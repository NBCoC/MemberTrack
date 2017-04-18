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

    public class PersonService : IPersonService, ISystemPersonService
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
                    _context.People.Include(p => p.CheckLists).
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
                            x => x.FullName.ToLower().Contains(contains)).
                        Take(25).
                        OrderBy(x => x.FullName).
                        ToListAsync();

            return new SearchResultDto<PersonSearchDto>(query.ToDtos(), count);
        }

        public async Task<long> SystemInsert(PersonInsertOrUpdateDto dto)
        {
            var entity = dto.ToEntity();

            _context.People.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<long> Insert(string contextUserEmail, PersonInsertOrUpdateDto dto)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(PersonInsertOrUpdateDto));
            }

            return await SystemInsert(dto);
        }

        public async Task<long> SystemEnsurePersonExists(PersonInsertOrUpdateDto personDto)
        {
            //This code should run in a transaction at the controller level, since we 'find' and then possibly 'insert'

            //This method uses the email address and person's name to identify them.
            //NOTE:  This is an area where we may get duplicates.  For instance, someone may have different first names (i.e. nicknames, shortened names)
            //       that they go by.

            var person = await Find(p => personDto.Email == p.Email && personDto.FullName == p.FullName);

            if (person == null)
            {
                return await SystemInsert(personDto);
            }

            return person.Id;
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

            entity.FullName = dto.FullName;
            entity.Email = dto.Email;
            entity.MembershipDate = dto.MembershipDate;
            entity.FirstVisitDate = dto.FirstVisitDate;
            entity.ContactNumber = dto.ContactNumber;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.AgeGroup = dto.AgeGroup;

            if (!entity.FirstVisitDate.HasValue &&
                entity.Status == PersonStatusEnum.Visitor)
            {
                entity.FirstVisitDate = DateTimeOffset.UtcNow;
            }

            if (!entity.MembershipDate.HasValue &&
                entity.Status == PersonStatusEnum.Member)
            {
                entity.MembershipDate = DateTimeOffset.UtcNow;
            }

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
                        Select(x => new { x.MembershipDate }).
                        GroupBy(x => new { x.MembershipDate.Value.Year, x.MembershipDate.Value.Month }).
                        ToListAsync();

            foreach (var group in memberGroups)
            {
                var item = dto.Items.FirstOrDefault(x => x.Month == group.Key.Month);

                if (item == null)
                    continue;

                if (group.Key.Month == date.Month &&
                    group.Key.Year < date.Year)
                    continue;

                item.MemberCount += group.Count();
            }

            var visitorGroups =
                await
                    _context.People.Where(
                            x => (x.FirstVisitDate != null) && x.FirstVisitDate <= date && x.FirstVisitDate >= lastYear).
                        Select(x => new { x.FirstVisitDate }).
                        GroupBy(x => new { x.FirstVisitDate.Value.Year, x.FirstVisitDate.Value.Month }).
                        ToListAsync();

            foreach (var group in visitorGroups)
            {
                var item = dto.Items.FirstOrDefault(x => x.Month == group.Key.Month);

                if (item == null)
                    continue;

                if (group.Key.Month == date.Month &&
                    group.Key.Year < date.Year)
                    continue;

                item.VisitorCount += group.Count();
            }

            return dto;
        }

        public async Task<IEnumerable<RecentPersonDto>> GetRecentActivity()
        {
            var result = new List<RecentPersonDto>();

            var recentVisitors =
                await
                    _context.People.Include(x => x.CheckLists).
                        ThenInclude(x => x.PersonCheckListItem).
                        Where(x => x.Status == PersonStatusEnum.Visitor && x.FirstVisitDate != null).
                        Select(
                            p =>
                                new RecentPersonDto
                                {
                                    Id = p.Id,
                                    Name = p.FullName,
                                    Status = p.Status,
                                    Date = p.FirstVisitDate,
                                    RequiresAttention =
                                        p.CheckLists.Count(
                                            cl => cl.PersonCheckListItem.Status == PersonStatusEnum.Visitor) !=
                                        _context.PersonCheckListItems.Count(
                                            pcli => pcli.Status == PersonStatusEnum.Visitor),
                                    LastModifiedDate =
                                        p.CheckLists.OrderBy(cl => cl.Date).Select(cl => cl.Date).FirstOrDefault()
                                }).
                        Where(p => p.RequiresAttention).
                        OrderBy(p => p.LastModifiedDate).
                        Take(10).
                        ToListAsync();

            var recentMembers =
                await
                    _context.People.Include(x => x.CheckLists).
                        ThenInclude(x => x.PersonCheckListItem).
                        Where(x => x.Status == PersonStatusEnum.Member && x.MembershipDate != null).
                        Select(
                            p =>
                                new RecentPersonDto
                                {
                                    Id = p.Id,
                                    Name = p.FullName,
                                    Status = p.Status,
                                    Date = p.MembershipDate,
                                    RequiresAttention =
                                        p.CheckLists.Count(
                                            cl => cl.PersonCheckListItem.Status == PersonStatusEnum.Member) !=
                                        _context.PersonCheckListItems.Count(
                                            pcli => pcli.Status == PersonStatusEnum.Member),
                                    LastModifiedDate =
                                        p.CheckLists.OrderBy(cl => cl.Date).Select(cl => cl.Date).FirstOrDefault()
                                }).
                        Where(p => p.RequiresAttention).
                        OrderBy(p => p.LastModifiedDate).
                        Take(10).
                        ToListAsync();

            result.AddRange(recentVisitors);
            result.AddRange(recentMembers);

            return result;
        }
    }
}