using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class Answer : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long QuestionId { get; set; }

        public Question Question { get; set; }

        public long TopicId { get; set; }

        public Topic Topic { get; set; }

        public int TopicWeight { get; set; }
    }
}
