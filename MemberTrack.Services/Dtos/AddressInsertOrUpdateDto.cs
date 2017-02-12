using MemberTrack.Data.Entities;

namespace MemberTrack.Services.Dtos
{
    public class AddressInsertOrUpdateDto
    {
        public string Street { get; set; }

        public string City { get; set; }

        public int ZipCode { get; set; }

        public StateEnum State { get; set; }
    }
}