using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Dtos
{
    public class QuizNextQuestionDto
    {
        public QuizQuestionDto NextQuestion { get; set; }

        public string PreviousQuestionAnswerResponse { get; set; }
    }
}
