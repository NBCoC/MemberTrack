using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Exceptions
{
    public class QuizNotFoundException : Exception
    {
        public QuizNotFoundException(long quizId) : base($"Quiz with ID {quizId} not found")
        {
            EntityId = quizId;
        }

        public QuizNotFoundException() : base("Quiz not found") { }

        public long? EntityId { get; private set; }
    }
}
