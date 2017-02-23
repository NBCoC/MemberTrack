using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MemberTrack.Common;
using MemberTrack.Common.Contracts;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MemberTrack.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IHashProvider _hashProvider;

        public UserService(DatabaseContext context, IHashProvider hashProvider)
        {
            _context = context;
            _hashProvider = hashProvider;
        }

        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

        public async Task<IDbContextTransaction> BeginTransactionAsync(
                CancellationToken cancellationToken = new CancellationToken())
            => await _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task<UserDto> Find(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var entity = await _context.Users.FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            return entity.ToDto();
        }

        public async Task<long> Insert(string contextUserEmail, UserInsertDto dto)
        {
            await ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Admin);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(UserInsertDto));
            }

            var entity = dto.ToEntity();

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Delete(string contextUserEmail, long userId)
        {
            await ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Admin);

            var entity =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !SystemAccountHelper.IsSystemAccount(u.Id));

            if (entity == null)
            {
                throw new EntityNotFoundException(userId);
            }

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Update(string contextUserEmail, UserUpdateDto dto, long userId)
        {
            await ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Admin);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(UserUpdateDto));
            }

            var entity =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !SystemAccountHelper.IsSystemAccount(u.Id));

            if (entity == null)
            {
                throw new EntityNotFoundException(userId);
            }

            entity.DisplayName = dto.DisplayName;
            entity.Role = dto.Role;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> Where(Expression<Func<User, bool>> predicate = null)
        {
            if (predicate == null)
            {
                predicate = x => !string.IsNullOrEmpty(x.Email);
            }

            var entities = await _context.Users.Where(predicate).ToListAsync();

            return entities.ToDtos();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var hashedPassword = _hashProvider.Hash(password);

            var entity =
                await
                    _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(hashedPassword));

            return entity != null;
        }

        public async Task ResetPassword(string contextUserEmail, long userId)
        {
            await ThrowIfNotInRole(contextUserEmail, UserRoleEnum.SystemAdmin);

            var entity =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !SystemAccountHelper.IsSystemAccount(u.Id));

            if (entity == null)
            {
                throw new EntityNotFoundException(userId);
            }

            entity.Password = SystemAccountHelper.DefaultPassword;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassword(string contextUserEmail, UserUpdatePasswordDto dto, long userId)
        {
            await ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Viewer);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(UserUpdatePasswordDto));
            }

            if (dto.OldPassword.Equals(dto.NewPassword))
            {
                throw new BadRequestException("Please provide a different password.");
            }

            var hashedPassword = _hashProvider.Hash(dto.OldPassword);

            var entity =
                await
                    _context.Users.FirstOrDefaultAsync(
                        u => u.Id == userId && u.Password.Equals(hashedPassword) && !SystemAccountHelper.IsSystemAccount(u.Id));

            if (entity == null)
            {
                throw new EntityNotFoundException(userId);
            }

            if (!entity.Email.Equals(contextUserEmail))
            {
                throw new UnauthorizeException();
            }

            entity.Password = _hashProvider.Hash(dto.NewPassword);

            await _context.SaveChangesAsync();
        }

        public async Task ThrowIfNotInRole(string email, UserRoleEnum role)
        {
            if (SystemAccountHelper.IsSystemAccount(email)) return;

            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            if (role == UserRoleEnum.Viewer)
            {
                if (entity.Role == UserRoleEnum.Viewer || entity.Role == UserRoleEnum.Editor ||
                    entity.Role == UserRoleEnum.Admin) return;
            }

            if (role == UserRoleEnum.Editor)
            {
                if (entity.Role == UserRoleEnum.Editor || entity.Role == UserRoleEnum.Admin) return;
            }

            if (role == UserRoleEnum.Admin && entity.Role == UserRoleEnum.Admin) return;

            throw new UnauthorizeException();
        }
    }
}