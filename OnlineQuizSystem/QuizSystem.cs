using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OnlineQuizSystem
{
    // Main class used to run the console-based online quiz system.
    public class QuizSystem
    {
        // Declaration and initialisation of lists used to store system data in memory.
        private static readonly List<Admin> admins = new();
        private static readonly List<Student> students = new();
        private static readonly List<Category> categories = new();
        private static readonly List<Quiz> quizzes = new();
        private static readonly List<Result> results = new();
        private static readonly List<Feedback> feedbacks = new();

        // Declaration and initialisation of counters used to generate IDs.
        private static int nextUserId = 1;
        private static int nextCategoryId = 1;
        private static int nextQuizId = 1;
        private static int nextQuestionId = 1;

        // Properties used to build file paths for storing CSV data.
        private static string DataDir => Path.Combine(AppContext.BaseDirectory, "Data");
        private static string QuestionsCsv => Path.Combine(DataDir, "questions.csv");
        private static string ResultsCsv => Path.Combine(DataDir, "results.csv");
        private static string FeedbackCsv => Path.Combine(DataDir, "feedback.csv");

        // Entry point used to start the program.
        public static void Main()
        {
            Console.Title = "Ulster University - Online Quiz System (COM326)"; // Setting console title.
            Directory.CreateDirectory(DataDir); // Creating data directory if it does not exist.

            SeedCoreData(); // Seeding core quizzes, categories, and users.
            EnsureQuestionsFilePresent(); // Ensuring questions.csv exists in output folder.
            LoadQuestionsFromCsv(QuestionsCsv); // Loading questions into quizzes from CSV.
            LoadResultsFromCsv(ResultsCsv); // Loading past results from CSV.
            LoadFeedbackFromCsv(FeedbackCsv); // Loading past feedback from CSV.

            MainMenu(); // Starting main menu loop.
        }

        // Method used to show the main menu and route the user to admin or student flows.
        public static void MainMenu()
        {
            while (true) // Looping until user chooses Exit.
            {
                Console.Clear(); // Clearing screen.
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Student");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine() ?? ""; // Reading user choice.

                // Routing based on choice.
                switch (choice)
                {
                    case "1":
                        AdminLoginMenu(); // Opening admin login menu.
                        break;
                    case "2":
                        StudentEntryMenu(); // Opening student entry menu.
                        break;
                    case "0":
                        return; // Exiting program.
                    default:
                        Pause("Invalid choice."); // Informing user of invalid input.
                        break;
                }
            }
        }

        // Method used to authenticate an admin before accessing admin menu.
        private static void AdminLoginMenu()
        {
            Console.Clear(); // Clearing screen.
            Console.WriteLine("Admin Login (Authentication Required)\n");

            Console.Write("Admin ID: ");
            int id = ReadInt(1, int.MaxValue); // Reading admin ID.

            Console.Write("Password: ");
            string pw = ReadNonEmpty(); // Reading password.

            var admin = admins.FirstOrDefault(a => a.UserId == id); // Searching for admin by ID.
            if (admin == null)
            {
                Pause("Admin not found."); // Informing user if admin does not exist.
                return; // Returning to main menu.
            }

            // Attempting login using stored username + entered password.
            if (!admin.Login(admin.UserName, pw))
            {
                Pause("Access denied."); // Informing user login failed.
                return; // Returning to main menu.
            }

            admin.LoginDate = DateTime.Now; // Storing login date/time.
            AdminMenu(admin); // Opening admin menu.
        }

        // Method used to show the admin menu and route admin actions.
        private static void AdminMenu(Admin admin)
        {
            while (true) // Looping until admin logs out.
            {
                Console.Clear(); // Clearing screen.
                Console.WriteLine($"Admin Panel (Logged in: {admin.UserName}, Last Login: {admin.LoginDate})\n");

                Console.WriteLine("1. Manage Questions");
                Console.WriteLine("2. Manage Users (Students)");
                Console.WriteLine("3. Manage Categories");
                Console.WriteLine("4. Export all questions to CSV");
                Console.WriteLine("5. View Results & Feedback");
                Console.WriteLine("0. Logout");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine() ?? ""; // Reading menu input.

                // Routing based on choice.
                switch (choice)
                {
                    case "1":
                        AdminManageQuestionsMenu(admin); // Opening question management.
                        break;
                    case "2":
                        AdminManageUsersMenu(admin); // Opening user management.
                        break;
                    case "3":
                        AdminManageCategoriesMenu(admin); // Opening category management.
                        break;
                    case "4":
                        admin.ExportQuestionsToCsv(quizzes, QuestionsCsv); // Exporting questions to CSV.
                        Pause($"Exported to: {QuestionsCsv}");
                        break;
                    case "5":
                        AdminViewResultsFeedback(); // Showing results and feedback.
                        break;
                    case "0":
                        admin.Logout(); // Logging out admin.
                        return; // Returning to main menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to manage questions in a selected quiz.
        private static void AdminManageQuestionsMenu(Admin admin)
        {
            Quiz quiz = SelectQuiz(); // Getting quiz chosen by admin.
            if (quiz == null) return; // Returning if admin cancelled quiz selection.

            while (true) // Looping until admin goes back.
            {
                Console.Clear();
                Console.WriteLine($"Manage Questions - {quiz.QuizTitle} ({quiz.QuizCategory.CategoryName})\n");

                Console.WriteLine("1. Show all questions");
                Console.WriteLine("2. Add new question");
                Console.WriteLine("3. Update a question");
                Console.WriteLine("4. Remove a question");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose: ");

                string choice = Console.ReadLine() ?? ""; // Reading choice.

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        admin.ShowQuestions(quiz); // Displaying questions.
                        Pause();
                        break;
                    case "2":
                        var q = BuildQuestionFromInput(); // Creating question from user input.
                        admin.AddQuestion(quiz, q); // Adding question to quiz.
                        if (q.QuestionID >= nextQuestionId) nextQuestionId = q.QuestionID + 1; // Updating next question ID.
                        Pause("Question added.");
                        break;
                    case "3":
                        UpdateQuestionFlow(admin, quiz); // Running update flow.
                        break;
                    case "4":
                        RemoveQuestionFlow(admin, quiz); // Running remove flow.
                        break;
                    case "0":
                        return; // Returning to admin menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to update a question inside a quiz.
        private static void UpdateQuestionFlow(Admin admin, Quiz quiz)
        {
            Console.Clear();
            admin.ShowQuestions(quiz); // Showing current questions.

            int qid = ReadIntPrompt("QuestionID to update: ", 1, int.MaxValue); // Reading question ID.

            Console.Write("New text (blank keep): ");
            string newText = Console.ReadLine() ?? ""; // Reading new question text.

            Console.Write("New difficulty (blank keep): ");
            string newDiff = Console.ReadLine() ?? ""; // Reading new difficulty.

            List<string>? newOptions = null; // Declaring options list.
            if (YesNoPrompt("Update options? (y/n): "))
            {
                newOptions = new List<string>(); // Initialising new options list.
                for (int i = 1; i <= 4; i++)
                    newOptions.Add(ReadNonEmptyPrompt($"Option {i}: ")); // Populating options list.
            }

            Console.Write("New correct answer (blank keep): ");
            string newCorrect = Console.ReadLine() ?? ""; // Reading new correct answer.

            bool ok = admin.UpdateQuestion(quiz, qid, newText, newDiff, newOptions, newCorrect); // Performing update.
            Pause(ok ? "Updated." : "Question not found."); // Outputting result message.
        }

        // Method used to remove a question from a quiz.
        private static void RemoveQuestionFlow(Admin admin, Quiz quiz)
        {
            Console.Clear();
            admin.ShowQuestions(quiz); // Showing current questions.

            int qid = ReadIntPrompt("QuestionID to remove: ", 1, int.MaxValue); // Reading question ID.
            bool ok = admin.RemoveQuestion(quiz, qid); // Removing question.
            Pause(ok ? "Removed." : "Not found."); // Outputting result message.
        }

        // Method used to manage student accounts.
        private static void AdminManageUsersMenu(Admin admin)
        {
            while (true) // Looping until admin goes back.
            {
                Console.Clear();
                Console.WriteLine("Manage Students\n");

                Console.WriteLine("1. Show all users");
                Console.WriteLine("2. Add student");
                Console.WriteLine("3. Update student");
                Console.WriteLine("4. Remove student");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose: ");

                string choice = Console.ReadLine() ?? ""; // Reading choice.

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        admin.ShowUsers(admins, students); // Displaying all users.
                        Pause();
                        break;
                    case "2":
                        AddStudentFlow(); // Running add student flow.
                        break;
                    case "3":
                        UpdateStudentFlow(admin); // Running update student flow.
                        break;
                    case "4":
                        RemoveStudentFlow(); // Running remove student flow.
                        break;
                    case "0":
                        return; // Returning to admin menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to add a new student account.
        private static void AddStudentFlow()
        {
            Console.Clear();
            Console.WriteLine("Add Student\n");

            string u = ReadNonEmptyPrompt("Username: "); // Reading username.
            string p = ReadNonEmptyPrompt("Password: "); // Reading password.
            string e = ReadNonEmptyPrompt("Email: "); // Reading email.
            string st = ReadStatusPrompt("Status (active/inactive): "); // Reading status.

            Admin.AddStudent(students, new Student(nextUserId++, u, p, e, st)); // Creating and adding student.
            Pause("Student added.");
        }

        // Method used to update an existing student account.
        private static void UpdateStudentFlow(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Update Student\n");

            int id = ReadIntPrompt("Student ID: ", 1, int.MaxValue); // Reading student ID.

            Console.Write("Username (blank keep): ");
            string u = Console.ReadLine() ?? ""; // Reading username.

            Console.Write("Email (blank keep): ");
            string e = Console.ReadLine() ?? ""; // Reading email.

            Console.Write("Password (blank keep): ");
            string p = Console.ReadLine() ?? ""; // Reading password.

            Console.Write("Status (blank keep): ");
            string st = Console.ReadLine() ?? ""; // Reading status.

            bool ok = admin.UpdateStudent(students, id, u, e, p, st); // Updating student.
            Pause(ok ? "Updated." : "Student not found.");
        }

        // Method used to remove a student account.
        private static void RemoveStudentFlow()
        {
            Console.Clear();
            Console.WriteLine("Remove Student\n");

            int id = ReadIntPrompt("Student ID: ", 1, int.MaxValue); // Reading student ID.
            bool ok = Admin.RemoveStudent(students, id); // Removing student.
            Pause(ok ? "Removed." : "Student not found.");
        }

        // Method used to manage categories.
        private static void AdminManageCategoriesMenu(Admin admin)
        {
            while (true) // Looping until admin goes back.
            {
                Console.Clear();
                Console.WriteLine("Manage Categories\n");

                Console.WriteLine("1. Show categories");
                Console.WriteLine("2. Add category");
                Console.WriteLine("3. Update category");
                Console.WriteLine("4. Remove category (only if unused)");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose: ");

                string choice = Console.ReadLine() ?? ""; // Reading choice.

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        admin.ShowCategories(categories); // Displaying categories.
                        Pause();
                        break;
                    case "2":
                        AddCategoryFlow(admin); // Running add category flow.
                        break;
                    case "3":
                        UpdateCategoryFlow(admin); // Running update category flow.
                        break;
                    case "4":
                        RemoveCategoryFlow(admin); // Running remove category flow.
                        break;
                    case "0":
                        return; // Returning to admin menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to add a category.
        private static void AddCategoryFlow(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Add Category\n");

            string name = ReadNonEmptyPrompt("Name: "); // Reading name.
            string desc = ReadNonEmptyPrompt("Description: "); // Reading description.

            admin.AddCategory(categories, new Category(nextCategoryId++, name, desc)); // Creating and adding category.
            Pause("Category added.");
        }

        // Method used to update a category.
        private static void UpdateCategoryFlow(Admin admin)
        {
            Console.Clear();
            admin.ShowCategories(categories); // Showing categories.

            int id = ReadIntPrompt("\nCategoryID: ", 1, int.MaxValue); // Reading category ID.

            Console.Write("Name (blank keep): ");
            string name = Console.ReadLine() ?? ""; // Reading name.

            Console.Write("Description (blank keep): ");
            string desc = Console.ReadLine() ?? ""; // Reading description.

            bool ok = admin.UpdateCategory(categories, id, name, desc); // Updating category.
            Pause(ok ? "Updated." : "Category not found.");
        }

        // Method used to remove a category when it's unused.
        private static void RemoveCategoryFlow(Admin admin)
        {
            Console.Clear();
            admin.ShowCategories(categories); // Showing categories.

            int id = ReadIntPrompt("\nCategoryID to remove: ", 1, int.MaxValue); // Reading category ID.
            bool ok = admin.RemoveCategory(categories, quizzes, id); // Removing category.
            Pause(ok ? "Removed." : "Remove failed (category may be used by a quiz).");
        }

        // Method used to display recent results and feedback to the admin.
        private static void AdminViewResultsFeedback()
        {
            Console.Clear();
            Console.WriteLine("Results:\n");

            // Checking if results list is empty.
            if (results.Count == 0) Console.WriteLine("No results yet.");
            else
            {
                // Looping through recent results.
                foreach (var r in results.OrderByDescending(x => x.Timestamp).Take(25))
                {
                    Console.WriteLine($"{r.Timestamp:g} | Student {r.StudentId} ({r.StudentName}) | Quiz {r.QuizId} {r.QuizTitle} | {r.CorrectAnswers}/{r.TotalQuestions} ({r.Percentage():0.00}%)");
                }
            }

            Console.WriteLine("\nFeedback:\n");

            // Checking if feedback list is empty.
            if (feedbacks.Count == 0) Console.WriteLine("No feedback yet.");
            else
            {
                // Looping through recent feedback.
                foreach (var f in feedbacks.OrderByDescending(x => x.Timestamp).Take(25))
                {
                    Console.WriteLine($"{f.Timestamp:g} | Student {f.StudentId} | Quiz {f.QuizId} | Rating {f.Rating}/5 | {f.Comment}");
                }
            }

            Pause(); // Waiting for admin to continue.
        }

        // Method used to allow students to either log in or continue as guest.
        private static void StudentEntryMenu()
        {
            while (true) // Looping until user goes back.
            {
                Console.Clear();
                Console.WriteLine("Student Menu");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Continue as Guest");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose: ");

                string choice = Console.ReadLine() ?? ""; // Reading choice.

                switch (choice)
                {
                    case "1":
                        StudentLoginFlow(); // Running login flow.
                        break;
                    case "2":
                        var guest = new Student(0, "Guest", "", "guest@local", "active"); // Creating guest account.
                        StudentMenu(guest); // Opening student menu as guest.
                        break;
                    case "0":
                        return; // Returning to main menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to log a student into the system.
        private static void StudentLoginFlow()
        {
            Console.Clear();
            Console.WriteLine("Student Login\n");

            Console.Write("Student ID: ");
            int id = ReadInt(1, int.MaxValue); // Reading student ID.

            Console.Write("Password: ");
            string pw = ReadNonEmpty(); // Reading password.

            var student = students.FirstOrDefault(s => s.UserId == id); // Searching for student by ID.
            if (student == null)
            {
                Pause("Student not found.");
                return;
            }

            // Checking if student is allowed to log in.
            if (!student.CanLogin())
            {
                Pause("Account is inactive. Contact admin.");
                return;
            }

            // Attempting login using stored username + entered password.
            if (!student.Login(student.UserName, pw))
            {
                Pause("Login failed.");
                return;
            }

            StudentMenu(student); // Opening student menu.
            student.Logout(); // Logging student out when finished.
        }

        // Method used to show the student menu and route student actions.
        private static void StudentMenu(Student student)
        {
            while (true) // Looping until student goes back.
            {
                Console.Clear();
                Console.WriteLine($"Student: {student.UserName} (ID: {student.UserId})");
                Console.WriteLine("1. Begin Quiz");
                Console.WriteLine("2. View My Results");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose: ");

                string choice = Console.ReadLine() ?? ""; // Reading choice.

                switch (choice)
                {
                    case "1":
                        BeginQuizFlow(student); // Starting quiz flow.
                        break;
                    case "2":
                        ViewMyResults(student); // Showing student results.
                        break;
                    case "0":
                        return; // Returning to student entry menu.
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        // Method used to display results for a specific student.
        private static void ViewMyResults(Student student)
        {
            Console.Clear();

            var my = results.Where(r => r.StudentId == student.UserId) // Filtering results by student ID.
                            .OrderByDescending(r => r.Timestamp) // Ordering by newest first.
                            .ToList();

            if (my.Count == 0)
            {
                Pause("No results yet.");
                return;
            }

            Console.WriteLine("My Results:\n");
            foreach (var r in my.Take(30)) // Looping through recent results.
                Console.WriteLine($"{r.Timestamp:g} | {r.QuizTitle} | {r.CorrectAnswers}/{r.TotalQuestions} ({r.Percentage():0.00}%)");

            Pause();
        }

        // Method used to run the quiz attempt flow.
        private static void BeginQuizFlow(Student student)
        {
            Category cat = SelectCategory(); // Getting category from user.
            if (cat == null) return; // Returning if selection was cancelled.

            var list = quizzes.Where(q => q.QuizCategory.CategoryID == cat.CategoryID) // Filtering quizzes by category.
                              .OrderBy(q => q.QuizID) // Sorting by quiz ID.
                              .ToList();

            if (list.Count == 0)
            {
                Pause("No quizzes in that category.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Quizzes in {cat.CategoryName}:\n");

            foreach (var q in list) // Looping through quizzes and outputting them.
                Console.WriteLine($"{q.QuizID}. {q.QuizTitle} (Questions: {q.QuizQuestions.Count})");

            int id = ReadIntPrompt("\nQuizID (0 cancel): ", 0, int.MaxValue); // Reading quiz selection.
            if (id == 0) return; // Returning if cancelled.

            var quiz = list.FirstOrDefault(q => q.QuizID == id); // Finding selected quiz.
            if (quiz == null || quiz.QuizQuestions.Count == 0)
            {
                Pause("Quiz not found or has no questions.");
                return;
            }

            var res = student.PlayQuiz(quiz); // Running the quiz attempt.
            results.Add(res); // Storing result in memory.
            SaveResultsToCsv(ResultsCsv); // Saving results to CSV.

            Console.Clear();
            Console.WriteLine("Quiz Complete!\n");
            Console.WriteLine($"Score: {res.CorrectAnswers}/{res.TotalQuestions}");
            Console.WriteLine($"Percentage: {res.Percentage():0.00}%");

            // Asking for feedback only for logged in students.
            if (student.UserId != 0 && YesNoPrompt("\nLeave feedback? (y/n): "))
            {
                int rating = ReadIntPrompt("Rating (1-5): ", 1, 5); // Reading rating.
                Console.Write("Comment: ");
                string comment = Console.ReadLine() ?? ""; // Reading comment.

                feedbacks.Add(new Feedback(DateTime.Now, student.UserId, quiz.QuizID, rating, comment)); // Creating feedback.
                SaveFeedbackToCsv(FeedbackCsv); // Saving feedback to CSV.

                Console.WriteLine("\nThanks for your feedback!");
            }

            Pause();
        }

        // Method used to allow admin to select a quiz from the list.
        private static Quiz? SelectQuiz()
        {
            Console.Clear();
            Console.WriteLine("Select a quiz:\n");

            foreach (var q in quizzes.OrderBy(q => q.QuizID)) // Looping through quizzes.
                Console.WriteLine($"{q.QuizID}. {q.QuizTitle} ({q.QuizCategory.CategoryName})");

            int id = ReadIntPrompt("\nQuizID (0 cancel): ", 0, int.MaxValue); // Reading quiz ID.
            if (id == 0) return null; // Returning null if cancelled.

            return quizzes.FirstOrDefault(q => q.QuizID == id); // Returning selected quiz or null.
        }

        // Method used to allow user to select a category from the list.
        private static Category? SelectCategory()
        {
            Console.Clear();
            Console.WriteLine("Select a category:\n");

            foreach (var c in categories.OrderBy(c => c.CategoryID)) // Looping through categories.
                Console.WriteLine($"{c.CategoryID}. {c.CategoryName}");

            int id = ReadIntPrompt("\nCategoryID (0 cancel): ", 0, int.MaxValue); // Reading category ID.
            if (id == 0) return null; // Returning null if cancelled.

            return categories.FirstOrDefault(c => c.CategoryID == id); // Returning selected category or null.
        }

        // Method used to build a new question object from console input.
        private static Question BuildQuestionFromInput()
        {
            Console.Clear();
            Console.WriteLine("Add Question\n");

            string text = ReadNonEmptyPrompt("Question text: "); // Reading question text.
            string diff = ReadNonEmptyPrompt("Difficulty: "); // Reading difficulty level.

            var options = new List<string>(); // Creating list to store options.
            for (int i = 1; i <= 4; i++)
                options.Add(ReadNonEmptyPrompt($"Option {i}: ")); // Populating options list.

            string correct = ReadNonEmptyPrompt("Correct answer (must exactly match an option): "); // Reading correct answer.
            if (!options.Contains(correct))
                correct = options[0]; // Defaulting to option 1 if not matching.

            return new Question(nextQuestionId++, text, options, correct, diff); // Returning new question object.
        }

        // Method used to seed initial categories, quizzes, and sample accounts.
        private static void SeedCoreData()
        {
            categories.Clear(); // Clearing categories list.
            quizzes.Clear(); // Clearing quizzes list.
            admins.Clear(); // Clearing admins list.
            students.Clear(); // Clearing students list.

            // Creating categories.
            categories.Add(new Category(nextCategoryId++, "Programming Concepts", "OOP and coding principles"));
            categories.Add(new Category(nextCategoryId++, "Data Structures", "Arrays, lists, stacks, queues, trees"));
            categories.Add(new Category(nextCategoryId++, "Software Design", "Design patterns, architecture, modelling"));
            categories.Add(new Category(nextCategoryId++, "Web Development", "HTML, CSS, JavaScript"));
            categories.Add(new Category(nextCategoryId++, "Database Systems", "SQL, relational models, normalization"));
            categories.Add(new Category(nextCategoryId++, "Cybersecurity Basics", "Encryption, authentication, threats"));
            categories.Add(new Category(nextCategoryId++, "Computer Networks", "Protocols, IP, routing, layers"));

            // Creating sample users.
            admins.Add(new Admin(nextUserId++, "admin", "admin123", "admin@ulster.ac.uk"));
            students.Add(new Student(nextUserId++, "student1", "pass123", "student1@ulster.ac.uk", "active"));

            // Creating dates used by quizzes.
            DateTime d1 = ParseDate("01/09/2025");
            DateTime d2 = ParseDate("07/09/2025");
            DateTime d3 = ParseDate("11/09/2025");
            DateTime d4 = ParseDate("13/09/2025");

            // Getting category references.
            Category prog = categories.First(c => c.CategoryName == "Programming Concepts");
            Category ds = categories.First(c => c.CategoryName == "Data Structures");
            Category sd = categories.First(c => c.CategoryName == "Software Design");
            Category wd = categories.First(c => c.CategoryName == "Web Development");
            Category db = categories.First(c => c.CategoryName == "Database Systems");
            Category cy = categories.First(c => c.CategoryName == "Cybersecurity Basics");
            Category nw = categories.First(c => c.CategoryName == "Computer Networks");

            // Creating quizzes.
            quizzes.Add(new Quiz(nextQuizId++, "OOP Fundamentals", "Basics of object-oriented programming", prog, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Data Structures", "Arrays, lists, stacks, queues, trees", ds, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Software Design", "Patterns, architecture, modelling", sd, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Web Development", "HTML, CSS, JavaScript", wd, new List<Question>(), d2));
            quizzes.Add(new Quiz(nextQuizId++, "Database Systems", "SQL, normalization, transactions", db, new List<Question>(), d2));
            quizzes.Add(new Quiz(nextQuizId++, "Cybersecurity Basics", "Encryption, authentication, threats", cy, new List<Question>(), d3));
            quizzes.Add(new Quiz(nextQuizId++, "Computer Networks", "Protocols, addressing, routing", nw, new List<Question>(), d4));
        }

        // Method used to parse a date in dd/MM/yyyy format.
        private static DateTime ParseDate(string ddMMyyyy)
            => DateTime.ParseExact(ddMMyyyy, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        // Method used to ensure questions.csv exists in the output Data directory.
        private static void EnsureQuestionsFilePresent()
        {
            if (File.Exists(QuestionsCsv)) return; // Returning if output file already exists.

            string baseDir = AppContext.BaseDirectory; // Getting output directory.
            var candidates = new List<string>(); // Creating list used to store candidate source paths.

            var dir = new DirectoryInfo(baseDir); // Creating DirectoryInfo for traversal.

            // Looping up the directory tree to search for a project Data/questions.csv.
            for (int i = 0; i < 6 && dir != null; i++)
            {
                candidates.Add(Path.Combine(dir.FullName, "Data", "questions.csv")); // Adding candidate path.
                dir = dir.Parent; // Moving to parent directory.
            }

            string? source = candidates.FirstOrDefault(File.Exists); // Finding first existing candidate.

            if (source != null)
            {
                Directory.CreateDirectory(DataDir); // Creating output Data directory.
                File.Copy(source, QuestionsCsv, true); // Copying questions.csv into output.
            }
            else
            {
                // Creating a new CSV file with header if no source was found.
                File.WriteAllLines(QuestionsCsv, new[]
                {
                    "QuizID,QuestionID,QuestionText,Opt1,Opt2,Opt3,Opt4,CorrectAnswer,Difficulty"
                });
            }
        }

        // Method used to load questions from a CSV file into quizzes.
        private static void LoadQuestionsFromCsv(string csvPath)
        {
            if (!File.Exists(csvPath)) return; // Returning if file does not exist.

            using var reader = new StreamReader(csvPath); // Creating StreamReader.
            reader.ReadLine(); // Reading and discarding header line.

            int maxQid = nextQuestionId - 1; // Declaration and initialisation of max question ID.

            while (!reader.EndOfStream) // Looping through file until end.
            {
                string? line = reader.ReadLine(); // Reading current line.
                if (string.IsNullOrWhiteSpace(line)) continue; // Skipping empty lines.

                var f = CsvUtil.ParseLine(line); // Parsing CSV fields.
                if (f.Count < 9) continue; // Skipping invalid rows.

                if (!int.TryParse(f[0], out int quizId)) continue; // Parsing quiz ID.
                if (!int.TryParse(f[1], out int qid)) continue; // Parsing question ID.

                var quiz = quizzes.FirstOrDefault(q => q.QuizID == quizId); // Finding quiz.
                if (quiz == null) continue; // Skipping if quiz not found.

                var opts = new List<string> { f[3], f[4], f[5], f[6] }; // Populating options list.
                var q = new Question(qid, f[2], opts, f[7], f[8]); // Creating question object.

                quiz.AddQuestion(q); // Adding question to quiz.

                if (qid > maxQid) maxQid = qid; // Updating max question ID.
            }

            nextQuestionId = maxQid + 1; // Updating next question ID.
        }

        // Method used to load results from results.csv.
        private static void LoadResultsFromCsv(string path)
        {
            if (!File.Exists(path)) return; // Returning if results file does not exist.

            using var reader = new StreamReader(path); // Creating StreamReader.
            reader.ReadLine(); // Reading and discarding header line.

            while (!reader.EndOfStream) // Looping through results file.
            {
                var line = reader.ReadLine(); // Reading current line.
                if (string.IsNullOrWhiteSpace(line)) continue; // Skipping empty lines.

                var f = CsvUtil.ParseLine(line); // Parsing CSV line.
                if (f.Count < 9) continue; // Skipping invalid rows.

                if (!DateTime.TryParse(f[0], null, DateTimeStyles.RoundtripKind, out var ts)) continue; // Parsing timestamp.
                if (!int.TryParse(f[1], out int sid)) continue; // Parsing student ID.

                string sname = f[2]; // Getting student name.

                if (!int.TryParse(f[3], out int qid)) continue; // Parsing quiz ID.
                string qtitle = f[4]; // Getting quiz title.

                if (!int.TryParse(f[5], out int total)) continue; // Parsing total questions.
                if (!int.TryParse(f[6], out int correct)) continue; // Parsing correct answers.

                results.Add(new Result(sid, sname, qid, qtitle, total, correct, ts)); // Creating and storing result.
            }
        }

        // Method used to save results to results.csv.
        private static void SaveResultsToCsv(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!); // Creating directory if needed.

            var lines = new List<string> { Result.CsvHeader() }; // Creating list with header row.
            lines.AddRange(results.OrderBy(r => r.Timestamp).Select(r => r.ToCsvLine())); // Adding result lines (Thanks again Stack Overflow).

            File.WriteAllLines(path, lines); // Writing results file.
        }

        // Method used to load feedback from feedback.csv.
        private static void LoadFeedbackFromCsv(string path)
        {
            if (!File.Exists(path)) return; // Returning if feedback file does not exist.

            using var reader = new StreamReader(path); // Creating StreamReader.
            reader.ReadLine(); // Reading and discarding header line.

            while (!reader.EndOfStream) // Looping through feedback file.
            {
                var line = reader.ReadLine(); // Reading current line.
                if (string.IsNullOrWhiteSpace(line)) continue; // Skipping empty lines.

                var f = CsvUtil.ParseLine(line); // Parsing CSV line.
                if (f.Count < 5) continue; // Skipping invalid rows.

                if (!DateTime.TryParse(f[0], null, DateTimeStyles.RoundtripKind, out var ts)) continue; // Parsing timestamp.
                if (!int.TryParse(f[1], out int sid)) continue; // Parsing student ID.
                if (!int.TryParse(f[2], out int qid)) continue; // Parsing quiz ID.
                if (!int.TryParse(f[3], out int rating)) continue; // Parsing rating.

                feedbacks.Add(new Feedback(ts, sid, qid, rating, f[4])); // Creating and storing feedback.
            }
        }

        // Method used to save feedback to feedback.csv.
        private static void SaveFeedbackToCsv(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!); // Creating directory if needed.

            var lines = new List<string> { Feedback.CsvHeader() }; // Creating list with header row.
            lines.AddRange(feedbacks.OrderBy(f => f.Timestamp).Select(f => f.ToCsvLine())); // Adding feedback lines.

            File.WriteAllLines(path, lines); // Writing feedback file.
        }

        // Method used to read an integer input with a prompt and range validation.
        private static int ReadIntPrompt(string prompt, int min, int max)
        {
            Console.Write(prompt); // Outputting prompt.
            return ReadInt(min, max, prompt); // Returning validated integer.
        }

        // Method used to read a non-empty string input with a prompt.
        private static string ReadNonEmptyPrompt(string prompt)
        {
            Console.Write(prompt); // Outputting prompt.
            return ReadNonEmpty(); // Returning validated string.
        }

        // Method used to read a yes/no response.
        private static bool YesNoPrompt(string prompt)
        {
            Console.Write(prompt); // Outputting prompt.
            string s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant(); // Reading and normalising input.
            return s == "y" || s == "yes"; // Returning true if user typed yes.
        }

        // Method used to read a status value with a prompt.
        private static string ReadStatusPrompt(string prompt)
        {
            Console.Write(prompt); // Outputting prompt.
            return ReadStatus(); // Returning validated status.
        }

        // Method used to read an integer input between a minimum and maximum value.
        private static int ReadInt(int min, int max, string retryPrompt)
        {
            while (true) // Looping until valid number is entered.
            {
                string s = (Console.ReadLine() ?? "").Trim(); // Reading nd Trimming user input.
                if (int.TryParse(s, out int v) && v >= min && v <= max)
                    return v; // Returning valid number.

                Console.Write($"Invalid Input. {retryPrompt}"); // Prompting again.
            }
        }

        // Method used to read a non-empty string input.
        private static string ReadNonEmpty()
        {
            while (true) // Looping until non-empty string is entered.
            {
                string s = (Console.ReadLine() ?? "").Trim(); // Reading and trimming input.
                if (s.Length > 0) return s; // Returning string if not empty.
                Console.Write("Cannot be empty: "); // Prompting again.
            }
        }

        // Method used to read a valid status value (active/inactive).
        private static string ReadStatus()
        {
            while (true) // Looping until valid status is entered.
            {
                string s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant(); // Reading and normalising input.
                if (s == "active" || s == "inactive") return s; // Returning valid status.
                Console.Write("Enter active or inactive: "); // Prompting again.
            }
        }

        // Method used to pause execution until the user presses ENTER.
        private static void Pause(string msg = "")
        {
            if (!string.IsNullOrWhiteSpace(msg))
                Console.WriteLine("\n" + msg); // Outputting message if provided.

            Console.WriteLine("\nPress ENTER..."); // Prompting ENTER.
            Console.ReadLine(); // Waiting for input.
        }
    }
}
