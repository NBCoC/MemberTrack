using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Dtos;

namespace MemberTrack.Services.Contracts
{
    public interface IPersonService
    {
        Task Delete(string contextUserEmail, long personId);

        Task<PersonDto> Find(Expression<Func<Person, bool>> predicate);

        Task<long> Insert(string contextUserEmail, PersonInsertOrUpdateDto dto);

        Task Update(string contextUserEmail, PersonInsertOrUpdateDto dto, long personId);

        Task InsertChildrenInfo(string contextUserEmail, ChildrenInfoDto dto, long personId);

        Task InsertOrUpdateVisit(string contextUserEmail, VisitDto dto, long personId);
    }
}