namespace MemberTrack.Data.Entities
{
    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public int ZipCode { get; set; }

        public StateEnum State { get; set; }

        public virtual Person Person { get; set; }

        public long PersonId { get; set; }
    }
}