using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class UserAnswer : IEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long AnswerId { get; set; }

        public Answer Answer { get; set; }

    }
}
