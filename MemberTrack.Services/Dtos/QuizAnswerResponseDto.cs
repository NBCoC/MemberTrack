using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Dtos
{
    public class QuizAnswerResponseDto
    {
        public long PersonId { get; set; }

        //This response is used by the UI to indicate that things like whether the person answered the question correctly or
        //if not, what the correct answer is.  
        //If empty, then implies that the UI has nothing to display in response to the answer provided.
        public string AnswerResponseText { get; set; }
    }
}
