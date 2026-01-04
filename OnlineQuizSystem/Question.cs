using System.Collections.Generic;

namespace OnlineQuizSystem
{
    // Class used to represent a multiple-choice question.
    public class Question
    {
        public int QuestionID { get; } // Storing unique question ID.
        public string QuestionText { get; set; } // Storing question text.
        public List<string> Options { get; set; } // Storing list of options.
        public string CorrectAnswer { get; set; } // Storing correct answer.
        public string DifficultyLevel { get; set; } // Storing difficulty level text.

        // Constructor used to create a question.
        public Question(int questionId, string questionText, List<string> options, string correctAnswer, string difficultyLevel)
        {
            QuestionID = questionId; // Populating question ID.
            QuestionText = questionText ?? ""; // Populating question text.
            Options = options ?? new List<string>(); // Populating options list.
            CorrectAnswer = correctAnswer ?? ""; // Populating correct answer.
            DifficultyLevel = difficultyLevel ?? ""; // Populating difficulty level.
        }

        // Method used to check if a chosen answer matches the correct answer.
        public bool IsCorrect(string answer)
            => answer == CorrectAnswer; // Returning true if answer is correct.
    }
}
