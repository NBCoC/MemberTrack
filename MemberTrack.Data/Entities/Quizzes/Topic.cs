using MemberTrack.Data.Contracts;
using System.Collections.Generic;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class Topic : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TopicCategory TopicCategory { get; set; }

        public long TopicCategoryId { get; set; }
        
        public List<Answer> Answers { get; set; }

        public List<SupportingScripture> SupportingScriptures { get; set; }
    }
}
