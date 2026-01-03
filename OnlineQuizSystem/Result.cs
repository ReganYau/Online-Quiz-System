using System;

namespace OnlineQuizSystem
{
    // Class used to represent the result of a quiz attempt.
    public class Result
    {
        public int StudentId { get; } // Storing student ID.
        public string StudentName { get; } // Storing student name.
        public int QuizId { get; } // Storing quiz ID.
        public string QuizTitle { get; } // Storing quiz title.
        public int TotalQuestions { get; } // Storing total number of questions.
        public int CorrectAnswers { get; } // Storing number of correct answers.
        public int WrongAnswers => TotalQuestions - CorrectAnswers; // Calculating number of wrong answers.
        public DateTime Timestamp { get; } // Storing when the attempt happened.

        public int Score => CorrectAnswers; // Storing score as number of correct answers.

        // Constructor used to create a result record.
        public Result(int studentId, string studentName, int quizId, string quizTitle, int totalQuestions, int correctAnswers, DateTime timestamp)
        {
            StudentId = studentId; // Populating student ID.
            StudentName = studentName ?? ""; // Populating student name.
            QuizId = quizId; // Populating quiz ID.
            QuizTitle = quizTitle ?? ""; // Populating quiz title.
            TotalQuestions = totalQuestions; // Populating total questions.
            CorrectAnswers = correctAnswers; // Populating correct answers.
            Timestamp = timestamp; // Populating timestamp.
        }

        // Method used to calculate the percentage score.
        public double Percentage()
        {
            if (TotalQuestions == 0) return 0; // Returnig 0 if there are no questions.
            return (CorrectAnswers * 100.0) / TotalQuestions; // Returning percentage value.
        }

        // Method used to convert result record into a CSV line.
        public string ToCsvLine()
        {
            return CsvUtil.Join(
                Timestamp.ToString("o"), // Writing timestamp in round-trip format.
                StudentId.ToString(), // Writing student ID.
                StudentName, // Writing student name.
                QuizId.ToString(), // Writing quiz ID.
                QuizTitle, // Writing quiz title.
                TotalQuestions.ToString(), // Writing total questions.
                CorrectAnswers.ToString(), // Writing correct answers.
                WrongAnswers.ToString(), // Writing wrong answers.
                Percentage().ToString("0.00") // Writing percentage to 2 decimal pointss.
            );
        }

        // Method used to return the header line for results CSV.
        public static string CsvHeader()
            => "Timestamp,StudentId,StudentName,QuizId,QuizTitle,TotalQuestions,CorrectAnswers,WrongAnswers,Percentage";
    }
}
