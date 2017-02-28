using MemberTrack.Data.Contracts;
using System.Collections.Generic;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class TopicCategory : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Topic> Topics { get; set; }
    }
}   
