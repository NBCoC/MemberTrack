using MemberTrack.Data.Entities;
using MemberTrack.Services.Dtos;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MemberTrack.Services.Contracts
{
    //This inferface allows for unauthenticated actions to be preformed on the system for Person entities.
    //NOTE:  Initially required to allow for a quiz (which has an unauthenticated user) to create a Person ("themselves")
    public interface ISystemPersonService
    {
        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default(CancellationToken));


        //Currently there is no authentication blockage, as any authenticated user can perform this method.
        //Therefore we don't need to create another method, but can reuse the one that IPersonService uses.
        Task<PersonDto> Find(Expression<Func<Person, bool>> predicate);

        //An unauthenticated Insert
        Task<long> SystemInsert(PersonInsertOrUpdateDto dto);

        Task<long> SystemEnsurePersonExists(PersonInsertOrUpdateDto personDto);

    }
}
