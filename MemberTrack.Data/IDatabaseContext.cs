using MemberTrack.Data.Entities;
using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data
{
    public interface IDatabaseContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Address> Addresses { get; set; }

        DbSet<Person> People { get; set; }

        DbSet<PersonCheckListItem> PersonCheckListItems { get; set; }

        DbSet<PersonCheckList> PersonCheckLists { get; set; }

        DbSet<Quiz> Quizzes { get; set; }

        DbSet<Question> QuizQuestions { get; set; }

        DbSet<Answer> QuizAnswers { get; set; }

        DbSet<TopicCategory> TopicCategories { get; set; }

        DbSet<Topic> Topics { get; set; }

        DbSet<SupportingScripture> SupportingScriptures { get; set; }

        DbSet<PersonAnswer> PersonAnswers { get; set; }

    }
}
