using MemberTrack.Common;

namespace MemberTrack.Services.Dtos
{
    public class AddressDto : AddressInsertOrUpdateDto
    {
        public string StateName => State.ToDescription();
    }
}