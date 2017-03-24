namespace MemberTrack.Data
{
    using Entities;
    using Entities.Quizzes;
    using Mappings;
    using Mappings.Quizzes;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        
        public DbSet<Person> People { get; set; }

        public DbSet<PersonCheckListItem> PersonCheckListItems { get; set; }

        public DbSet<PersonCheckList> PersonCheckLists { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Question> QuizQuestions { get; set; }

        public DbSet<Answer> QuizAnswers { get; set; }

        public DbSet<TopicCategory> TopicCategories { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<SupportingScripture> SupportingScriptures { get; set; }

        public DbSet<PersonAnswer> PersonAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            UserMap.Configure(builder);
            PersonMap.Configure(builder);
            PersonCheckListItemMap.Configure(builder);
            PersonCheckListMap.Configure(builder);
            QuizMap.Configure(builder);
            QuestionMap.Configure(builder);
            AnswerMap.Configure(builder);
            TopicCategoryMap.Configure(builder);
            TopicMap.Configure(builder);
            SupportingScriptureMap.Configure(builder);
            PersonAnswerMap.Configure(builder);
        }
    }
}