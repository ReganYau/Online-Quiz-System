using System;
using System.Collections.Generic;
using System.Linq;
using OnlineQuizSystem;
using Xunit;

namespace OnlineQuizSystem.Tests
{
    public class CoreTests
    {
        [Fact]
        public void CsvUtil_ParseLine_HandlesCommaInsideQuotes()
        {
            string line = "1,2,\"hello, world\",A,B,C,D,A,Easy";
            var fields = CsvUtil.ParseLine(line);

            Assert.Equal(9, fields.Count);
            Assert.Equal("hello, world", fields[2]);
        }

        [Fact]
        public void CsvUtil_Join_EscapesQuotesCorrectly()
        {
            string csv = CsvUtil.Join("a", "he said \"hi\"", "b");
            Assert.Equal("a,\"he said \"\"hi\"\"\",b", csv);
        }

        [Fact]
        public void Question_IsCorrect_ReturnsTrueOnlyForCorrectAnswer()
        {
            var q = new Question(
                questionId: 1,
                questionText: "Test",
                options: new List<string> { "A", "B", "C", "D" },
                correctAnswer: "C",
                difficultyLevel: "Easy"
            );

            Assert.True(q.IsCorrect("C"));
            Assert.False(q.IsCorrect("A"));
        }

        [Fact]
        public void Quiz_AddQuestion_DoesNotAddDuplicateQuestionId()
        {
            var cat = new Category(1, "Test", "Test");
            var quiz = new Quiz(1, "Quiz", "Desc", cat, new List<Question>(), DateTime.Today);

            quiz.AddQuestion(new Question(10, "Q1", new List<string> { "A", "B", "C", "D" }, "A", "Easy"));
            quiz.AddQuestion(new Question(10, "Q1 Duplicate", new List<string> { "A", "B", "C", "D" }, "A", "Easy"));

            Assert.Single(quiz.QuizQuestions);
        }

        [Fact]
        public void Admin_UpdateQuestion_UpdatesTextAndDifficulty()
        {
            var cat = new Category(1, "Test", "Test");
            var quiz = new Quiz(1, "Quiz", "Desc", cat, new List<Question>(), DateTime.Today);
            quiz.AddQuestion(new Question(5, "Old", new List<string> { "A", "B", "C", "D" }, "B", "Easy"));

            var admin = new Admin(99, "admin", "pw", "admin@test.com");

            bool ok = admin.UpdateQuestion(quiz, 5, "New Text", "Medium", null, null);

            Assert.True(ok);
            Assert.Equal("New Text", quiz.GetQuestion(5)!.QuestionText);
            Assert.Equal("Medium", quiz.GetQuestion(5)!.DifficultyLevel);
        }

        [Fact]
        public void Student_CanLogin_DependsOnStatus()
        {
            var active = new Student(1, "s1", "p", "e@e.com", "active");
            var inactive = new Student(2, "s2", "p", "e@e.com", "inactive");

            Assert.True(active.CanLogin());
            Assert.False(inactive.CanLogin());
        }

        [Fact]
        public void Admin_RemoveCategory_FailsIfCategoryInUse()
        {
            var categories = new List<Category> { new Category(1, "Cat", "Desc") };
            var quizzes = new List<Quiz>
            {
                new Quiz(1, "Quiz", "Desc", categories[0], new List<Question>(), DateTime.Today)
            };

            var admin = new Admin(1, "admin", "pw", "a@a.com");

            bool ok = admin.RemoveCategory(categories, quizzes, 1);

            Assert.False(ok);
            Assert.Single(categories);
        }
    }
}
