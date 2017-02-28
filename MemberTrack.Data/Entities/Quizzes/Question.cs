using MemberTrack.Data.Contracts;
using System.Collections.Generic;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class Question : IEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public bool RandomizeAnswers { get; set; }

        public bool AllowMultipleAnswers { get; set; }

        public long QuizId { get; set; }

        public Quiz Quiz { get; set; }    

        public List<Answer> Answers { get; set; }
    }
}
