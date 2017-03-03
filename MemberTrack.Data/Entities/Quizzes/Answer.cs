using MemberTrack.Data.Contracts;

namespace MemberTrack.Data.Entities.Quizzes
{
    public class Answer : IEntity
    {
        public long Id { get; set; }

        //They are interpolated strings.   For the Gifts quiz "{TopicWeight}", this is same as weight, i.e. 0, 1, 2, etc..
        //For another quiz "{Position}", A, B, C, D …  {LetterPosition} is a calculated value, since answers may be randomized.)
        public string Name { get; set; }

        public string Description { get; set; }

        public long QuestionId { get; set; }

        public Question Question { get; set; }

        public long? TopicId { get; set; }

        public Topic Topic { get; set; }

        //For the Gifts quiz, this is 0, 1, 2, etc… For traditional quizes just 0 or 1.
        public int TopicWeight { get; set; }
    }
}
