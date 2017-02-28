using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class SupportingScripture : IEntity
    {
        public long Id { get; set; }

        public string ScriptureReference { get; set; }

        public string Description { get; set; }

        public long TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}
