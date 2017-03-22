
using System.Collections.Generic;

namespace MemberTrack.Services.Dtos
{
    public class QuizQuestionDto
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public bool AllowMultipleAnswers { get; set; }

        public IEnumerable<QuizAnswerDto> Answers { get; set; } = new List<QuizAnswerDto>();        
    }
}
