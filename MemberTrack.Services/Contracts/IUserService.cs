using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Dtos;
using Microsoft.EntityFrameworkCore.Storage;

namespace MemberTrack.Services.Contracts
{
    public interface IUserService
    {
        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(string contextUserEmail, long userId);

        Task<UserDto> Find(Expression<Func<User, bool>> predicate);

        Task<IEnumerable<UserDto>> Where(Expression<Func<User, bool>> predicate = null);

        Task<long> Insert(string contextUserEmail, UserInsertDto dto);

        Task Update(string contextUserEmail, UserUpdateDto dto, long userId);

        Task<bool> Authenticate(string email, string password);

        Task ResetPassword(string contextUserEmail, long userId);

        Task UpdatePassword(string contextUserEmail, UserUpdatePasswordDto dto, long userId);

        Task ThrowIfNotInRole(string email, UserRoleEnum role);
    }
}