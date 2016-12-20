using System;
using System.Threading.Tasks;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Services
{
    public class AddressService : IAddressService
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public AddressService(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Delete(string contextUserEmail, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            var entity = await _context.Addresses.FirstOrDefaultAsync(a => a.PersonId == personId);

            if (entity == null)
            {
                throw new EntityNotFoundException(personId);
            }

            _context.Addresses.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task InsertOrUpdate(string contextUserEmail, AddressInsertOrUpdateDto dto, long personId)
        {
            await _userService.ThrowIfNotInRole(contextUserEmail, UserRoleEnum.Editor);

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(AddressInsertOrUpdateDto));
            }

            var entity = await _context.Addresses.FirstOrDefaultAsync(a => a.PersonId == personId);

            if (entity == null)
            {
                entity = dto.ToEntity();

                entity.PersonId = personId;

                _context.Add(entity);
            }
            else
            {
                entity.Street = dto.Street;
                entity.City = dto.City;
                entity.ZipCode = dto.ZipCode;
                entity.State = dto.State;
            }

            await _context.SaveChangesAsync();
        }
    }
}