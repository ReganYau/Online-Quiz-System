using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQuizSystem
{
    // Class used to represent a quiz containing a category, date, and list of questions.
    public class Quiz
    {
        public int QuizID { get; } // Storing unique quiz ID.
        public string QuizTitle { get; set; } // Storing quiz title.
        public string QuizDescription { get; set; } // Storing quiz description.
        public Category QuizCategory { get; set; } // Storing linked category.
        public List<Question> QuizQuestions { get; } // Storing list of questions in the quiz.
        public DateTime QuizDate { get; set; } // Storing quiz date.

        // Constructor used to create a quiz.
        public Quiz(int quizId, string title, string description, Category category, List<Question> questions, DateTime date)
        {
            QuizID = quizId; // Populating quiz ID.
            QuizTitle = title ?? ""; // Populating title.
            QuizDescription = description ?? ""; // Populating description.
            QuizCategory = category; // Populating category reference.
            QuizQuestions = questions ?? new List<Question>(); // Populating question list.
            QuizDate = date; // Populating quiz date.
        }

        // Method used to add a question into the quiz while avoiding duplicates.
        public void AddQuestion(Question q)
        {
            // Checking if the question already exists in the quiz.
            if (QuizQuestions.Any(x => x.QuestionID == q.QuestionID))
                return; // Returning without adding duplicate.

            QuizQuestions.Add(q); // Adding question to quiz.
        }

        // Method used to remove a question from the quiz by QuestionID.
        public bool RemoveQuestion(int questionId)
        {
            var q = QuizQuestions.FirstOrDefault(x => x.QuestionID == questionId); // Searching for question.
            if (q == null) return false; // Returning false if not found.

            QuizQuestions.Remove(q); // Removing question from list.
            return true; // Returning true if removal succeeded.
        }

        // Method used to get a question object from the quiz by QuestionID.
        public Question? GetQuestion(int questionId)
            => QuizQuestions.FirstOrDefault(x => x.QuestionID == questionId); // Returning matching question or null.
    }
}
