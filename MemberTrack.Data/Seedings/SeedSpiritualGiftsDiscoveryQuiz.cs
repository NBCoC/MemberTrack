using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Data.RawData;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MemberTrack.Data.Seedings
{
    public class SeedSpiritualGiftsDiscoveryQuiz : SeedBase
    {

        public SeedSpiritualGiftsDiscoveryQuiz(DatabaseContext databaseContext, bool forceReseeding)
            : base(databaseContext, forceReseeding)
        {
        }

        protected override void ClearData()
        {
            var existingQuiz = _databaseContext.Quizzes.Include(q => q.Questions).FirstOrDefault(q => q.Name == SpiritualGiftsDiscoveryQuizData.QuizName);

            //Is this quiz even in the database?
            if (existingQuiz != null)
            {
                _databaseContext.Remove(existingQuiz);
                _databaseContext.SaveChanges();
            }

            var existingTopicCategory = _databaseContext.TopicCategories.Include(t => t.Topics).FirstOrDefault(t => t.Name == SpiritualGiftsDiscoveryQuizData.TopicCategory);
            if (existingTopicCategory != null)
            {
                _databaseContext.Remove(existingTopicCategory);
                _databaseContext.SaveChanges();
            }
        }

        protected override void PopulateData()
        {
            var quizId = AddQuiz();

            var firstTopicId = AddTopics();

            AddQuestions(quizId, firstTopicId);
        }

        private long AddQuiz()
        {
            var quiz = new Quiz
            {
                Name = SpiritualGiftsDiscoveryQuizData.QuizName,
                Description = "Discovery your own spiritual gifts by taking this quiz.",
                RandomizeQuestions = true
            };

            _databaseContext.Quizzes.Add(quiz);
            _databaseContext.SaveChanges();
            return quiz.Id;
        }


        //The return value is the first topic ID.  All other topics are assuming to be sequencial after that number.
        private long AddTopics()
        {
            var topicCategory = new TopicCategory { Name = SpiritualGiftsDiscoveryQuizData.TopicCategory };
            _databaseContext.TopicCategories.Add(topicCategory);
            _databaseContext.SaveChanges();

            long firstTopicId = -1;
            foreach(var topicData in SpiritualGiftsDiscoveryQuizData.Topics)
            {          
                var topic = new Topic { Name = topicData.Item1, Description = topicData.Item2, TopicCategoryId = topicCategory.Id };
                _databaseContext.Topics.Add(topic);
                _databaseContext.SaveChanges();

                if (firstTopicId < 0)
                    firstTopicId = topic.Id;

                foreach (var supportingScriptureText in topicData.Item3)
                {
                    var supportingScripture = new SupportingScripture { ScriptureReference = supportingScriptureText, TopicId = topic.Id };
                    _databaseContext.SupportingScriptures.Add(supportingScripture);
                }
                _databaseContext.SaveChanges();
            }

            return firstTopicId;
        }

        private void AddQuestions(long quizId, long firstTopicId)
        {
            var questions = SpiritualGiftsDiscoveryQuizData.QuizQuestionText.Select(text => new Question
            {
                Description = text,
                RandomizeAnswers = false,
                AllowMultipleAnswers = false,
                QuizId = quizId,
            });

            int counter = 0;
            foreach(Question question in questions)
            {
                _databaseContext.QuizQuestions.Add(question);
                _databaseContext.SaveChanges();

                var topicId = firstTopicId + (counter % SpiritualGiftsDiscoveryQuizData.Topics.Count);
                AddAnswers(question.Id, topicId);
                counter++;
            }

        }


        private void AddAnswers(long questionId, long topicId)
        {
            //All questions in this quiz have the same answers, but it relates to different topics.
            int counter = 0;
            SpiritualGiftsDiscoveryQuizData.AnswersText.ForEach(answer =>
            {
                _databaseContext.QuizAnswers.Add(new Answer { Name = "{TopicWeight}", QuestionId = questionId, TopicId = topicId, TopicWeight = counter++, Description = answer });
            });

            _databaseContext.SaveChanges();
        }
    }
}
