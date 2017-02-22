using System.Threading;
using System.Threading.Tasks;
using MemberTrack.Services.Dtos;
using Microsoft.EntityFrameworkCore.Storage;

namespace MemberTrack.Services.Contracts
{
    public interface IAddressService
    {
        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(string contextUserEmail, long personId);

        Task InsertOrUpdate(string contextUserEmail, AddressInsertOrUpdateDto dto, long personId);
    }
}