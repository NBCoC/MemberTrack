using System;

namespace MemberTrack.Common.Quiz.Exceptions
{
    public class QuizAnswersNotSameQuestionException : Exception
    {
        public QuizAnswersNotSameQuestionException(long offendingAnswerId, long expectedQuestionId, long actualQuestionId) 
            : base($"Answer ID {offendingAnswerId} has Question ID of {actualQuestionId} instead of excepted ID of {expectedQuestionId}")
        {
            OffendingAnswerId = offendingAnswerId;
            ExpectedQuestionId = expectedQuestionId;
            ActualQuestionId = actualQuestionId;
        }

        public QuizAnswersNotSameQuestionException() : base("Answers aren't all for the same question.") { }

        public long? OffendingAnswerId { get; private set; }
        public long? ExpectedQuestionId { get; private set; }
        public long? ActualQuestionId { get; private set; }

    }
}
