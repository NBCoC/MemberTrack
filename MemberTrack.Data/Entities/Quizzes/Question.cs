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

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        //TODO: Need some type of interpolated string property or properties that is used to compose 
        //      QuizAnswerResponseDto.AnswerResponseText for either 'correct' or 'wrong' answers.
        //      (Isn't needed for Spiritual Gifts Discovery Quiz though)
        //      This may be even a whole new table, since there may be more responses then just 'right' and 'wrong', 
        //      but this all may be overkill for now.
    }
}
