using System.Threading.Tasks;
using MemberTrack.Services.Dtos;

namespace MemberTrack.Services.Contracts
{
    public interface IAddressService
    {
        Task Delete(string contextUserEmail, long personId);

        Task InsertOrUpdate(string contextUserEmail, AddressInsertOrUpdateDto dto, long personId);
    }
}