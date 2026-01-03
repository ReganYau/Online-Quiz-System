using System;

namespace OnlineQuizSystem
{
    // Class used to represent a student user who can attempt quizzes.
    public class Student : User
    {
        public string Status { get; set; } = "active"; // Storing account status.

        // Constructor used to create a student user.
        public Student(int id, string username, string password, string email, string status)
            : base(id, username, password, email, "student") // Calling base constructor to populate shared user fields.
        {
            Status = (status ?? "active").Trim().ToLowerInvariant(); // Normalising status input.
            if (Status != "active" && Status != "inactive") Status = "active"; // Defaulting invalid status to active.
        }

        // Method used to check if student account is allowed to log in.
        public bool CanLogin() => Status == "active"; // Returning true only if status is active.

        // Method used to run through a quiz attempt and return the final result.
        public Result PlayQuiz(Quiz quiz)
        {
            int total = quiz.QuizQuestions.Count; // Storing total number of questions.
            int correct = 0; // Declaration and initialisation of variable used to count correct answers.

            // Looping through each question in the quiz.
            for (int i = 0; i < quiz.QuizQuestions.Count; i++)
            {
                var q = quiz.QuizQuestions[i]; // Getting current question.

                Console.Clear(); // Clearing console for a clean question display.
                Console.WriteLine($"{quiz.QuizTitle} ({quiz.QuizCategory.CategoryName})"); // Outputting quiz title and category.
                Console.WriteLine($"Question {i + 1} of {total}"); //Outputting question number.
                Console.WriteLine(new string('-', 60)); // Outputting separator line.
                Console.WriteLine(q.QuestionText); // Outputting question text.
                Console.WriteLine(); // Outputting blank line.

// Creating a local list of options so the question data is not permanently changed.
var optionList = q.Options.Select(opt => new { Text = opt, IsCorrect = (opt == q.CorrectAnswer) }).ToList();

// Shuffling the local option list.
var rng = new Random();
optionList = optionList.OrderBy(x => rng.Next()).ToList();

// Displaying shuffled options.
for (int optIndex = 0; optIndex < optionList.Count; optIndex++)
    Console.WriteLine($"{optIndex + 1}. {optionList[optIndex].Text}");

Console.Write("\nYour answer (1-4): ");
int choice = ReadInt(1, 4);

// Checking if the chosen option is correct.
bool ok = optionList[choice - 1].IsCorrect;

                if (ok) correct++; // Increasing correct count if correct.

                Console.WriteLine(ok ? "\nCorrect!" : $"\nWrong! Correct answer: {q.CorrectAnswer}"); // Outputting result message.
                Console.WriteLine("\nPress ENTER..."); // Prompting user to continue.
                Console.ReadLine(); // Waiting for ENTER.
            }

            // Creating and returning the final result object.
            return new Result(
                studentId: this.UserId, // Using current student's ID.
                studentName: this.UserName, // Using current student's name.
                quizId: quiz.QuizID, // Using quiz ID.
                quizTitle: quiz.QuizTitle, // Using quiz title.
                totalQuestions: total, // Using total question count.
                correctAnswers: correct, // Using correct answer count.
                timestamp: DateTime.Now // Using current time.
            );
        }

        // Method used to read an integer between a minimum and maximum value.
        private static int ReadInt(int min, int max)
        {
            while (true) // Looping until valid number is entered.
            {
                string s = Console.ReadLine() ?? ""; // Reading user input.
                if (int.TryParse(s, out int v) && v >= min && v <= max)
                    return v; // Returning valid number.
            }
        }
    }
}
