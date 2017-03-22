using MemberTrack.Common.Quiz;
using MemberTrack.Data;
using MemberTrack.Data.Entities;
using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MemberTrack.Services.Tests
{
    /// <summary>
    /// Since the entity & DTO relationship is intensionally decoupled in the DtoExtenions class, these tests are used to ensure that when
    /// adding extra properties, they are copied in the conversions.
    /// </summary>
    [TestClass]
    public class DtoExtensionsTests
    {
        //This method checks to see that all the properties on the DTO have either a matching property on the entity, or if not then has its 
        //property name in the propertyExceptions with a Func<> that returns true.
        private void EnsureConversionThroughReflection<TSource, TDestination>(TSource source, TDestination destination, Dictionary<string, Func<bool>> propertyExceptions = null)
        {
            var entityProperties = source.GetType().GetProperties();

            foreach (PropertyInfo property in destination.GetType().GetProperties())
            {
                //See if this property has an exception, before comparing its values.
                Func<bool> propertyMatchFunc;
                if (propertyExceptions != null && propertyExceptions.TryGetValue(property.Name, out propertyMatchFunc))
                {
                    Assert.IsTrue(propertyMatchFunc(), $"class {destination.GetType().Name} with property named {property.Name} failed its property exception lambda");
                    continue;
                }

                var propertyValue = property.GetValue(destination);

                //Check to see if this DTO property has the same name property on the Entity
                var matchingEntityProperty = entityProperties.FirstOrDefault(p => p.Name == property.Name);
                if (matchingEntityProperty != null)
                {
                    var entityValue = matchingEntityProperty.GetValue(source);
                    Assert.AreEqual(entityValue, propertyValue, $"The two values of Property {property.Name} aren't equal.  Source value: {entityValue} Destination value: {propertyValue}");
                    continue;
                }

                //There isn't a matching property name, need to assert, so that possible new properties on the destination are added to the conversion or to the exceptions list.
                Assert.Fail($"class {destination.GetType().Name} doesn't have matching property (or property exception) named {property.Name} in class {source.GetType().Name}");
            }
        }

        [TestMethod]
        public void ToDto_Quiz_To_QuizDto()
        {
            var quizDto = QuizTestData.DisneyPrincessQuiz.ToDto();

            var propertyExceptions = new Dictionary<string, Func<bool>>
            {
                { nameof(QuizDto.QuestionCount), () => QuizTestData.DisneyPrincessQuiz.Questions.Count == quizDto.QuestionCount  }
            };

            EnsureConversionThroughReflection(QuizTestData.DisneyPrincessQuiz, quizDto, propertyExceptions);
        }

        [TestMethod]
        public void ToTerseDto_Quiz_To_QuizTerseDto()
        {
            var quizDto = QuizTestData.DisneyPrincessQuiz.ToTerseDto();

            var propertyExceptions = new Dictionary<string, Func<bool>>
            {
                { nameof(QuizTerseDto.QuestionCount), () => QuizTestData.DisneyPrincessQuiz.Questions.Count == quizDto.QuestionCount  }
            };

            EnsureConversionThroughReflection(QuizTestData.DisneyPrincessQuiz, quizDto, propertyExceptions);
        }

        [TestMethod]
        public void ToDto_User_To_UserDto()
        {
            var user = SystemAccountHelper.SystemAccounts.First();
            var userDto = user.ToDto();

            var propertyExceptions = new Dictionary<string, Func<bool>>
            {
                //Just ignore these for simplicity
                { nameof(UserDto.RoleName), () => true  },
                { nameof(UserDto.IsSystemAdmin), () => true  },
                { nameof(UserDto.IsAdmin), () => true  },
                { nameof(UserDto.IsEditor), () => true  },
            };

            EnsureConversionThroughReflection(user, userDto, propertyExceptions);
        }

        [TestMethod]
        public void ToEntity_UserInsertDto_To_User()
        {
            var userDto = new UserInsertDto
            {
                DisplayName = "Prince Solms",
                Email = "prince@newbraunfels.gov",
                Role = Data.Entities.UserRoleEnum.Admin
            };

            var user = userDto.ToEntity();

            var propertyExceptions = new Dictionary<string, Func<bool>>
            {
                { nameof(User.Id), () => true  },
                { nameof(User.Password), () => true  },
            };

            EnsureConversionThroughReflection(userDto, user, propertyExceptions);
        }

        [TestMethod]
        public void ToDto_Answer_To_QuizAnswerDto()
        {
            var answer = new Answer
            {
                Id = 10,
                Name = "I.",
                Description = "1971",
                TopicId = 123
            };

            var index = 5;
            var quizManager = new QuizManager();
            var quizAnswerDto = answer.ToDto(index, quizManager);

            //NOTE: The Name interpolation is tested on its own.
            EnsureConversionThroughReflection(answer, quizAnswerDto);
        }

        [TestMethod]
        public void ToDto_Question_To_QuizQuestionDto()
        {
            var answer = new Answer
            {
                Id = 10,
                Name = "a.",
                Description = "1744",
            };
            var answer2 = new Answer
            {
                Id = 11,
                Name = "b.",
                Description = "1544",
            };


            var question = new Question
            {
                Id = 5,
                Description = "When was the Alamo built?",
                AllowMultipleAnswers = false,
                Answers = new List<Answer> { answer, answer2 },
            };

            var quizManager = new QuizManager();
            var quizQuestionDto = question.ToDto(quizManager);

            var propertyExceptions = new Dictionary<string, Func<bool>>
            {
                //Don't interogate because QuizAnswerDto conversions are tested above.  Just make sure that it is filled with them.
                { nameof(QuizQuestionDto.Answers), () => quizQuestionDto.Answers.Count() == question.Answers.Count() },
            };

            EnsureConversionThroughReflection(question, quizQuestionDto, propertyExceptions);
        }

    }
}
