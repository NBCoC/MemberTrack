using System;

namespace MemberTrack.Common.Quiz.Exceptions
{
    public class QuizAnswersNotForQuizException : Exception
    {
        public QuizAnswersNotForQuizException(long offendingAnswerId, long expectedQuizId, long actualQuizId) 
            : base($"Answer ID {offendingAnswerId} is part of Quiz with ID of {actualQuizId} instead of excepted Quiz with ID of {expectedQuizId}")
        {
            OffendingAnswerId = offendingAnswerId;
            ExpectedQuizId = expectedQuizId;
            ActualQuizId = actualQuizId;
        }

        public QuizAnswersNotForQuizException() : base("Answers aren't for the expected quiz.") { }

        public long? OffendingAnswerId { get; private set; }
        public long? ExpectedQuizId { get; private set; }
        public long? ActualQuizId { get; private set; }

    }
}
