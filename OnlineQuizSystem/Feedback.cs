using System;

namespace OnlineQuizSystem
{
    // Class used to represent student feedback after completing a quiz.
    public class Feedback
    {
        public DateTime Timestamp { get; } // Storing when the feedback was submitted.
        public int StudentId { get; } // Storing the student ID linked to feedback.
        public int QuizId { get; } // Storing the quiz ID linked to feedback.
        public int Rating { get; } // Storing rating value (1 to 5).
        public string Comment { get; } // Storing the written comment.

        // Constructor used to create a feedback record.
        public Feedback(DateTime timestamp, int studentId, int quizId, int rating, string comment)
        {
            Timestamp = timestamp; // Populating timestamp.
            StudentId = studentId; // Populating student ID.
            QuizId = quizId; // Populating quiz ID.
            Rating = rating; // Populating rating.
            Comment = comment ?? ""; // Populating comment defaulting to empty string if null.
        }

        // Method used to convert feedback record into a CSV line.
        public string ToCsvLine()
        {
            return CsvUtil.Join(
                Timestamp.ToString("o"), // Writing timestamp in round-trip format (Thanks Stack Overflow).
                StudentId.ToString(), // Writing student ID.
                QuizId.ToString(), // Writing quiz ID.
                Rating.ToString(), // Writing rating.
                Comment // Writing comment.
            );
        }

        // Method used to return the header line for feedback CSV.
        public static string CsvHeader()
            => "Timestamp,StudentId,QuizId,Rating,Comment";
    }
}
