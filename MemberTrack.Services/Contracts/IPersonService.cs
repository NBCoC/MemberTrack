namespace MemberTrack.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Entities;
    using Dtos;
    using Microsoft.EntityFrameworkCore.Storage;

    public interface IPersonService
    {
        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(string contextUserEmail, long personId);

        Task<PersonDto> Find(Expression<Func<Person, bool>> predicate);

        Task<long> Insert(string contextUserEmail, PersonInsertOrUpdateDto dto);

        Task Update(string contextUserEmail, PersonInsertOrUpdateDto dto, long personId);

        Task UpdateDates(string contextUserEmail, DatesDto dto, long personId);

        Task InsertChildrenInfo(string contextUserEmail, ChildrenInfoDto dto, long personId);

        Task InsertOrRemoveCheckListItem(string contextUserEmail, PersonCheckListItemDto dto, long personId);

        Task<IEnumerable<PersonCheckListItemLookupDto>> GetCheckListItemLookup();

        Task<SearchResultDto<PersonSearchDto>> Search(string contains);

        Task<PersonReportDto> GetReport();
    }
}