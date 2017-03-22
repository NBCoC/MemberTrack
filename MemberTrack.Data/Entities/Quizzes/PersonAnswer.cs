using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class PersonAnswer : IEntity
    {
        public long Id { get; set; }

        public long PersonId { get; set; }

        public Person Person { get; set; }

        public long AnswerId { get; set; }

        public Answer Answer { get; set; }

    }
}
