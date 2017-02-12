namespace MemberTrack.Data.Entities
{
    public class DocumentData
    {
        public virtual Document Document { get; set; }

        public long DocumentId { get; set; }

        public byte[] Data { get; set; }
    }
}