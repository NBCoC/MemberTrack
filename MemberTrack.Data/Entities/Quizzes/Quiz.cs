using MemberTrack.Data.Contracts;
using System.Collections.Generic;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class Quiz : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //This is HTML markup
        public string Instructions { get; set; }

        public bool RandomizeQuestions { get; set; }

        public List<Question> Questions { get; set; }
    }
}
