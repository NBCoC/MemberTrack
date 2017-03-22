using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Dtos
{
    public class QuizQuestionAnswerDto
    {
        //This field is enumerable for quizzes that can have multiple answers for a question.
        public IEnumerable<long> AnswerIds { get; set; } = new List<long>();
    }
}
