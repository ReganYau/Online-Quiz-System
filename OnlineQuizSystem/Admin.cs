using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQuizSystem
{
    // Class used to represent an admin user with permission to manage systems.
    public class Admin : User
    {
        public DateTime LoginDate { get; set; } // Storing the date/time the admin last logged in.

        // Constructor used to create an admin user.
        public Admin(int id, string username, string password, string email)
            : base(id, username, password, email, "admin") // Calling base constructor to populate shared user fields.
        {
        }

        // Method used to display all questions in a quiz.
        public void ShowQuestions(Quiz quiz)
        {
            Console.WriteLine($"Quiz: {quiz.QuizTitle} ({quiz.QuizCategory.CategoryName})"); // Outputting quiz title and category.
            Console.WriteLine(new string('-', 60)); // Outputting separator line.

            // Checking if quiz has no questions.
            if (quiz.QuizQuestions.Count == 0)
            {
                Console.WriteLine("No questions."); // Informing user there are no questions.
                return; // Returning to caller.
            }

            // Looping through questions in order of QuestionID.
            foreach (var q in quiz.QuizQuestions.OrderBy(q => q.QuestionID))
            {
                Console.WriteLine($"QID {q.QuestionID}: {q.QuestionText} [{q.DifficultyLevel}]"); // Outputting question summary.

                // Looping through options for the current question.
                for (int i = 0; i < q.Options.Count; i++)
                    Console.WriteLine($"   {(i + 1)}. {q.Options[i]}"); // Outputting option number and text.

                Console.WriteLine($"   Correct: {q.CorrectAnswer}"); // Outputting correct answer.
                Console.WriteLine(); // Outputting blank line for spacing.
            }
        }

        // Method used to add a question into a quiz.
        public void AddQuestion(Quiz quiz, Question question)
        {
            quiz.AddQuestion(question); // Calling quiz method to add the question.
        }

        // Method used to remove a question from a quiz by QuestionID.
        public bool RemoveQuestion(Quiz quiz, int questionId)
        {
            return quiz.RemoveQuestion(questionId); // Returning result from quiz removal operation.
        }

        // Method used to update a question inside a quiz.
        public bool UpdateQuestion(
            Quiz quiz,
            int questionId,
            string? newText,
            string? newDifficulty,
            List<string>? newOptions,
            string? newCorrectAnswer)
        {
            var q = quiz.GetQuestion(questionId); // Getting question object from quiz.
            if (q == null) return false; // Returning false if question does not exist.

            // Updating question text if a new value was provided.
            if (!string.IsNullOrWhiteSpace(newText))
                q.QuestionText = newText.Trim(); // Trimming and storing new text.

            // Updating difficulty level if a new value was provided.
            if (!string.IsNullOrWhiteSpace(newDifficulty))
                q.DifficultyLevel = newDifficulty.Trim(); // Trimming and storing new difficulty.

            // Updating options if a full new set of 4 options was provided.
            if (newOptions != null && newOptions.Count == 4 && newOptions.All(o => !string.IsNullOrWhiteSpace(o)))
                q.Options = newOptions.Select(o => o.Trim()).ToList(); // Trimming and storing new options list.

            // Updating correct answer if a new value was provided and exists in options.
            if (!string.IsNullOrWhiteSpace(newCorrectAnswer))
            {
                string ca = newCorrectAnswer.Trim(); // Trimming new correct answer.
                if (q.Options.Contains(ca))
                    q.CorrectAnswer = ca; // Setting correct answer only if it matches an option.
            }

            return true; // Returning true if update succeeded.
        }

        // Method used to display all users..
        public void ShowUsers(List<Admin> admins, List<Student> students)
        {
            Console.WriteLine("Admins:"); // Outputting admin header.
            Console.WriteLine(new string('-', 60)); // Outputting separator line.

            // Looping through admins in ID order.
            foreach (var a in admins.OrderBy(a => a.UserId))
                Console.WriteLine($"{a.UserId} | {a.UserName} | {a.Email}"); // Outputting admin summary.

            Console.WriteLine(); // Outputting blank line.

            Console.WriteLine("Students:"); // Outputting student header.
            Console.WriteLine(new string('-', 60)); // Outputting separator line.

            // Looping through students in ID order.
            foreach (var s in students.OrderBy(s => s.UserId))
                Console.WriteLine($"{s.UserId} | {s.UserName} | {s.Email} | {s.Status}"); // Outputting student summary.
        }

        // Method used to add a new student into the student list.
        public static void AddStudent(List<Student> students, Student student)
        {
            students.Add(student); // Adding student to list.
        }

        // Method used to remove a student from the list by ID.
        public static bool RemoveStudent(List<Student> students, int id)
        {
            var s = students.FirstOrDefault(x => x.UserId == id); // Searching for student by ID.
            if (s == null) return false; // Returning false if student not found.

            students.Remove(s); // Removing student from list.
            return true; // Returning true if removal succeeded.
        }

        // Method used to update a student's details.
        public bool UpdateStudent(List<Student> students, int id, string? username, string? email, string? password, string? status)
        {
            var s = students.FirstOrDefault(x => x.UserId == id); // Searching for student by ID.
            if (s == null) return false; // Returning false if student not found.

            s.UpdateProfile(username, email, password); // Updating shared user fields.

            // Updating student status if provided and valid.
            if (!string.IsNullOrWhiteSpace(status))
            {
                string st = status.Trim().ToLowerInvariant(); // Normalising status input.
                if (st == "active" || st == "inactive")
                    s.Status = st; // Storing validated status.
            }

            return true; // Returning true if update succeeded.
        }

        // Method used to display all categories.
        public void ShowCategories(List<Category> categories)
        {
            Console.WriteLine("Categories:"); // Outputting category header.
            Console.WriteLine(new string('-', 60)); // Outputting separator line.

            // Looping through categories in ID order.
            foreach (var c in categories.OrderBy(c => c.CategoryID))
                Console.WriteLine($"{c.CategoryID}. {c.CategoryName} - {c.CategoryDescription}"); // Outputting category summary.
        }

        // Method used to add a new category into the category list.
        public void AddCategory(List<Category> categories, Category category)
        {
            categories.Add(category); // Adding category to list.
        }

        // Method used to update a category.
        public bool UpdateCategory(List<Category> categories, int categoryId, string? name, string? desc)
        {
            var c = categories.FirstOrDefault(x => x.CategoryID == categoryId); // Searching for category by ID.
            if (c == null) return false; // Returning false if category not found.

            // Updating category name if a new value was provided.
            if (!string.IsNullOrWhiteSpace(name)) c.CategoryName = name.Trim();

            // Updating category description if a new value was provided.
            if (!string.IsNullOrWhiteSpace(desc)) c.CategoryDescription = desc.Trim();

            return true; // Returning true if update succeeded.
        }

        // Method used to remove a category onl if no quizzes are using it.
        public bool RemoveCategory(List<Category> categories, List<Quiz> quizzes, int categoryId)
        {
            // Checking if any quiz is using the category.
            if (quizzes.Any(q => q.QuizCategory.CategoryID == categoryId))
                return false; // Returning false if category is in use.

            var c = categories.FirstOrDefault(x => x.CategoryID == categoryId); // Searching for category.
            if (c == null) return false; // Returning false if category not found.

            categories.Remove(c); // Removing category from list.
            return true; // Returning true if removal succeeded.
        }

        // Method used to export all quiz questions into a CSV file.
        public void ExportQuestionsToCsv(List<Quiz> quizzes, string csvPath)
        {
            // Creating list used to hold output lines for CSV.
            var lines = new List<string>
            {
                "QuizID,QuestionID,QuestionText,Opt1,Opt2,Opt3,Opt4,CorrectAnswer,Difficulty" // CSV header row.
            };

            // Looping through quizzes in ID order.
            foreach (var quiz in quizzes.OrderBy(q => q.QuizID))
            {
                // Looping through each question in the quiz in QuestionID order.
                foreach (var q in quiz.QuizQuestions.OrderBy(x => x.QuestionID))
                {
                    // Adding a CSV row for the question.
                    lines.Add(CsvUtil.Join(
                        quiz.QuizID.ToString(),
                        q.QuestionID.ToString(),
                        q.QuestionText,
                        q.Options.ElementAtOrDefault(0) ?? "",
                        q.Options.ElementAtOrDefault(1) ?? "",
                        q.Options.ElementAtOrDefault(2) ?? "",
                        q.Options.ElementAtOrDefault(3) ?? "",
                        q.CorrectAnswer,
                        q.DifficultyLevel
                    ));
                }
            }

            // Creating destination folder if it does not exist.
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(csvPath)!);

            // Writing all CSV lines to file.
            System.IO.File.WriteAllLines(csvPath, lines);
        }
    }
}
