using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Dtos
{
    public class QuizDto : QuizTerseDto
    {
        //This is HTML markup
        public string Instructions { get; set; }

    }
}
