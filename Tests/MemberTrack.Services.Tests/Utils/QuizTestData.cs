using MemberTrack.Data;
using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Tests.Utils
{
    static class QuizTestData
    {
        public static readonly Quiz WeightQuiz = new Quiz
        {
            Name = "How to loose weight",
            Description = "Taking this quiz is so exhaustive that you'll surely loose weight here in 20 minutes!",
            RandomizeQuestions = false,
            Instructions = "div id='shell - header' class='shell - header shell - responsive ' ms.pgarea='header' role='banner' data-ckrate='0.005'></div>"
        };

        public static readonly Quiz PersonalityQuiz = new Quiz
        {
            Name = "Reveal your main personality type",
            Description = "See which of our eight Myers-Briggs-inspired categories you fall into.",
            RandomizeQuestions = true,
            Instructions = "<img _cid='CanWeGuessYourAgeBasedOnJust30Questions' src='http://www3.pictures.zimbio.com/mp/iTYvPHqPlbXs.jpg' data-pin-nopin='true'>"
        };

        public static readonly Quiz DisneyPrincessQuiz = new Quiz
        {
            Name = "Can We Guess Your Favorite Disney Princess Based on Your Taste in Movies?",
            Description = "I can show you this quiz — shining, shimmering, splendid.",
            RandomizeQuestions = true,
            Instructions = "<a href=' / quiz / mUNWh4JVDVz / Can + Guess + Favorite + Disney + Princess + Based + Taste' class='title _c' _cid='CanWeGuessYourFavoriteDisneyPrincessBasedOnYourTasteInMovies'>Can We Guess Your Favorite Disney Princess Based on Your Taste in Movies?</a>"
        };

        public static DatabaseContext GetPopulatedDatabase()
        {
            var dbContextBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            //Provide an in-memory database who has a GUID for a name, to isolate each test.
            dbContextBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var databaseContext = new DatabaseContext(dbContextBuilder.Options);

            //Add some quizzes.
            databaseContext.Quizzes.Add(WeightQuiz);
            databaseContext.Quizzes.Add(PersonalityQuiz);
            databaseContext.Quizzes.Add(DisneyPrincessQuiz);

            databaseContext.SaveChanges();

            return databaseContext;
        }

    }
}
