using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Dtos
{
    public class QuizAnswerDto
    {
        public long Id { get; set; }

        //Actual value after interpolation.
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
