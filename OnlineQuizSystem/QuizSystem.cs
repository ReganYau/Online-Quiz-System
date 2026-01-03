using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class QuizSystem
    {
        // Private Fields
        private static List<Admin> admins = new List<Admin>();
        private static List<Student> students = new List<Student>();
        private static List<Category> categories = new List<Category>();
        private static List<Quiz> quizzes = new List<Quiz>();

        private int nextUserId = 1;
        private int nextCategoryId = 1;
        private int nextQuizID = 1;
        private int nextQuestionId = 1;

        public static void Main()
        {
            Console.Title = "Ulster University - Online Quiz System (COM326)";

            string folder = "Data";
            string filename = "questions.csv";

            destinationFilePath = CopyDataToWorkingDir("Data", "questions.csv");

            SeedCoreData();
            LoadQuestionsFromCsv(destinationFilePath);
            MainMenu();
        }
        // Main Menu
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Student");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminLoginMenu();
                        break;
                    case "2":
                        StudentMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("invalid choice. press enter to try again");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // Admin Menu
        public static void AdminLoginMenu()
        {
            Console.Clear();

            Console.WriteLine("Admin Login (Authenticatiion Required)\n");
            Console.Write("Admin ID: ");
            int id = ReadInt(1, int.MaxValue);

            Console.Write("Password: ");
            string pw = ReadNonEmpty();

            Admin admin = admins.FirstOrDefault(a => a.UserId == id);

            if (admin == null)
            {
                Console.WriteLine("\nAdmin not found. Press enter to return.");
                Console.ReadLine();
                return;
            }

            bool ok = admin.Login(admin.UserName, pw);
            if (!ok)
            {
                Console.WriteLine("\nAccess denied. Press enter to return");
                Console.ReadLine();
                return;
            }

            admin.LoginDate = DateTime.Now;

            AdminLoginMenu(admin);
        }

        private static void AdminMenu(Admin admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Admin Panel (Logged in: {admin.UserName}, Last Login: {admin.LoginDate})\n");

                Console.WriteLine("1. Manage Questions (Add/Remove/Update/Show)");
                Console.WriteLine("2. Manage Users (Add/Remove/Update/Show)");
                Console.WriteLine("3. Manage Categories (Add/Remove/Update/Show");
                Console.WriteLine("4. Save all quizzes to csv (questions.csv)");
                Console.WriteLine("0. Logout");
                Console.Write("\n Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminManageQuestionsMenu(admin);
                        break;
                    case "2":
                        AdminManageUsersMenu(admin);
                        break;
                    case "3":
                        AdminManageCategoriesMenu(admin);
                        break;
                    case "4":
                        ExportFlow(admin);
                        break;
                    case "0":
                        admin.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void AdminManageQuestionsMenu(Admin admin)
        {
            Quiz quiz = SelectQuiz();
            if (quiz == null) return;
                     
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Manage Questions - {quiz.QuizTitle} ({quiz.QuizCategory.CategoryName})\n");

                Console.WriteLine("1. Show all questions");
                Console.WriteLine("2. Add new questions");
                Console.WriteLine("3. Update a question");
                Console.WriteLine("4. Remove a question");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowQuestions(admin, quiz);
                        Pause();
                        break;
                    case "2":
                        AddQuestion(admin, quiz);
                        break;
                    case "3":
                        UpdateQuestionInQuiz(admin, quiz);
                        break;
                    case "4":
                        RemoveQuestionFromQuiz(admin, quiz);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void AdminManageUsersMenu(Admin admin)
        {
            while  (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Users\n");

                Console.WriteLine("1. Show all users");
                Console.WriteLine("2. Add student");
                Console.WriteLine("3. Update student (username/email/password/status)");
                Console.WriteLine("4. Remove student");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllUsers(admin);
                        break;
                    case "2":
                        AddStudents(admin);
                        break;
                    case "3":
                        UpdateStudents(admin);
                        break;
                    case "4":
                        RemoveStudents(admin);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void AdminManageCategoriesMenu(Admin admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Categories\n");

                Console.WriteLine("1. Show categories");
                Console.WriteLine("2. Add category");
                Console.WriteLine("3. Update category");
                Console.WriteLine("4. Remove category (only if no quizzes use it)");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowCategoriesFlow(admin);
                        break;
                    case "2":
                        AddCategories(admin);
                        break;
                    case "3":
                        UpdateCategories(admin);
                        break;
                    case "4":
                        RemoveCategories(admin);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Student Menu:");
                Console.WriteLine("1. Begin Quiz");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BeginQuizFlow();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void ExportFlow (Admin admin)
        {
            admin.ExportQuestionsToCsv(quizzes, "questions.csv");
            Pause("Exported to questions.csv");
        }

        private static void ShowQuestion(Admin admin, Quiz quiz)
        {
            Console.Clear();
            admin.ShowQuestions(quiz);
            Pause();
        }

        private static void AddQuestion(Admin admin, Quiz quiz)
        {
            Question q = BuildQuestionFromInput();
            admin.AddQuestion(quiz, q);
            Pause("Question added.");
        }

        private static void UpdateQuestionInQuiz(Admin admin, Quiz quiz)
        {
            admin.ShowQuestions(quiz);

            int qid = ReadIntPrompt("QuestionID: ", 1, int.MaxValue);

            Console.Write("New text (blank to keep): ");
            string text = Console.ReadLine();

            Console.Write("New difficulty (blank to keep): ");
            string diff = Console.ReadLine();

            List<string> options = null;
            if (YesNoPrompt("Update all options? (y/n): "))
            {
                options = new List<string>();
                for (int i = 0; i < qid; i++)
                    options.Add(ReadNonEmptyPrompt($"Option {i}: "));
            }

            Console.Write("New correct answer (blank to keep): ");
            string correct = Console.ReadLine();

            bool ok = admin.UpdateQuestion(quiz, qid, text, diff, options, correct);
            Pause(ok ? "Question update." : "Update failed.");
        }

        private static void RemoveQuestionFromQuiz(Admin admin, Quiz quiz)
        {
            Console.Clear();
            admin.ShowQuestions(quiz);

            int qid = ReadIntPrompt("QuestionID to remove: ", 1, int.MaxValue);
            bool ok = admin.RemoveQuestion(quiz, qid);

            Pause(ok ? "Question removed." : "Question not found.");
        }

        private static void ShowAllUsers(Admin admin)
        {
            Console.Clear();
            admin.ShowUsers(admins, students);
            Pause();
        }

        private static void AddStudents()
        {
            Console.Clear();
            Console.WriteLine("\nAdd Student");

            string u = ReadNonEmptyPrompt("Username: ");
            string p = ReadNonEmptyPrompt("Password: ");
            string e = ReadNonEmptyPrompt("Email: ");
            string st = ReadStatusPrompt("Status (active/inactive): ");

            Admin.AddStudent(students, new Student(nextUserId++, u, p, e, st));
            Pause("Student added.");
        }

        private static void UpdateStudents(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("\nUpdate Students");

            int id = ReadIntPrompt("Student ID: ", 1, int.MaxValue);

            Console.Write("Username (blank to keep) ");
            string u = Console.ReadLine();

            Console.Write("Email (blank to keep) ");
            string e = Console.ReadLine();

            Console.Write("Password (blank to keep) ");
            string p = Console.ReadLine();

            Console.Write("Status (blank to keep) ");
            string st = Console.ReadLine();

            bool ok = admin.UpdateStudent(students, id, u, e, p, st);
            Pause(ok ? "Student updated." : "Student not found.");
        }

        public static void RemoveStudents()
        {
            Console.Clear();
            Console.WriteLine("\nRemove Student");

            int id = ReadIntPrompt("Student ID: ", 1, int.MaxValue);

            bool ok = Admin.RemoveStudent(students, id);
            Pause(ok ? "Student removed." : "Student not found.");
        }

        private static void ShowCategoriesFlow(Admin admin)
        {
            Console.Clear();
            admin.ShowCategories(categories);
            Pause();
        }

        private static void AddCategories(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Add category");

            string name = ReadNonEmptyPrompt("Name: ");
            string desc = ReadNonEmptyPrompt("Description: ");

            admin.AddCategory(categories, new Category(nextCategoryId++, name, desc));
            Pause("Category added.");
        }

        private static void UpdateCategories(Admin admin)
        {
            Console.Clear();
            admin.ShowCategories(categories);

            int id = ReadIntPrompt("\nCategoryID: ", 1, int.MaxValue);

            Console.Write("Name (blank to keep): ");
            string name = Console.ReadLine();

            Console.Write("Description (blank to keep): ");
            string desc = Console.ReadLine();

            bool ok = admin.UpdateCategory(categories, id, name, desc);
            Pause(ok ? "Category Updated." : "Category not found.");
        }

        private static void RemoveCategories(Admin admin)
        {
            Console.Clear();
            admin.ShowCategories(categories);

            int id = ReadIntPrompt("\nCategoryID to remove: ", 1, int.MaxValue);

            bool ok = admin.RemoveCategory(categories, quizzes, id);
            Pause(ok ? "Category removed." : "Remove failed");
        }

        private static void BeginQuizFlow ()
        {
            Category cat = SelectCategory();
            if (cat == null) 
                return;

            List<Quiz> list = quizzes.Where(q => q.QuizCategory.CategoryID == cat.CategoryID).ToList();
            if (list.Count == 0)
            {
                Pause("No quizzes in that category.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Quizzes in {cat.CategoryName}:\n");
            foreach (var q in list)
                Console.WriteLine($"{q.QuizID}. {q.QuizTitle} (Q: {q.QuizQuestions.Count})");

            int id = ReadIntPrompt("\nQuizID (0 to cancel): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Quiz quiz = list.FirstOrDefault(q => q.QuizID == id);
            if (quiz == null || quiz.QuizQuestions.Count == 0)
            {
                Pause("Quiz not found pr has no questions.");
                return;
            }

            Student guest = new Student(0, "Guest", "", "guest@local", "active");

            Console.Clear();
            Console.WriteLine($"Starting: {quiz.QuizTitle}\n");
            guest.PlayQuiz(quiz);
            Pause();
        }

        private static Quiz SelectQuiz()
        {
            Console.Clear();
            Console.WriteLine("Select a quiz:\n");
            foreach (var q in quizzes.OrderBy(q => q.QuizID))
                Console.WriteLine($"{q.QuizID}. {q.QuizTitle} ({q.QuizCategory.CategoryName})");

            int id = ReadIntPrompt("\nQuizID (0 to cancel): ", 0, int.MaxValue);
            if (id == 0)
                return null;

            return quizzes.FirstOrDefault(q => q.QuizID == id);
        }

        private static Category SelectCategory()
        {
            Console.Clear();
            Console.WriteLine("Select a category\n");

            foreach (var c in categories.OrderBy(c => c.CategoryID))
                Console.WriteLine($"{c.CategoryID}. {c.CategoryName}");

            int id = ReadIntPrompt("\nCategoryID (0 to cancel) ", 0, int.MaxValue);
            if (id == 0)
                return null;

            return categories.FirstOrDefault(c => c.CategoryID == id);
        }

        private static Question BuildQuestionFromInput()
        {
            Console.Clear();
            Console.WriteLine("Add question\n");

            string text = ReadNonEmptyPrompt("Question text: ");
            string diff = ReadNonEmptyPrompt("Difficulty: ");

            List<string> options = new List<string>();
            for (int i = 1; i <= 4; i++)
                options.Add(ReadNonEmptyPrompt($"Option {i}: "));

            string correct = ReadNonEmptyPrompt("Correct answer (must match from an option): ");
            if (!options.Contains(correct)) correct = options[0];

            return new Question(nextQuestionId++, text, options, correct, diff);
        }

        public static string CopyDataToWorkingDir(string folder, string filename)
        {
            string projectDirectory = Path.GetDirectoryName(folder);
            string sourceFilePath = Path.Combine(projectDirectory, filename);
            string destinationFilePath = ;

            if (File.Exists(sourceFilePath))
                File.Copy(sourceFilePath, destinationFilePath, true);
            else
                Console.WriteLine("Source file not found: " + sourceFilePath);

            return destinationFilePath;
        }

        private static void LoadQuestionsFromCsv(string csvPath)
        {
            if (!File.Exists(csvPath)) return;

            using (var reader = new StreamReader(csvPath))
            {
                reader.ReadLine(); // header
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    List<string> f = ParseCsvLine(line);
                    if (f.Count < 9) continue;

                    int quizId = int.Parse(f[0]);
                    int qid = int.Parse(f[1]);

                    Quiz quiz = quizzes.FirstOrDefault(q => q.QuizID == quizId);
                    if (quiz == null) continue;

                    var opts = new List<string> { f[3], f[4], f[5], f[6] };
                    quiz.AddQuestion(new Question(qid, f[2], opts, f[7], f[8]));

                    if (qid >= nextQuestionId) nextQuestionId = qid + 1;
                }
            }
        }

        private static List<string> ParseCsvLine(string line)
        {
            List<string> r = new List<string>();
            bool inQuotes = false;
            string cur = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') { cur += '"'; i++; }
                    else inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    r.Add(cur);
                    cur = "";
                }
                else cur += c;
            }
            r.Add(cur);
            return r;
        }

        private static void SeedCoreData()
        {
            categories.Add(new Category(nextCategoryId++, "Programming Concepts", "Concepts of object-oriented programming and coding principles"));
            categories.Add(new Category(nextCategoryId++, "Data Structures", "Arrays, lists, stacks, queues, trees, and their applications"));
            categories.Add(new Category(nextCategoryId++, "Software Design", "Design patterns, architecture principles, and system modelling"));
            categories.Add(new Category(nextCategoryId++, "Web Development", "HTML, CSS, JavaScript, and client-server interactions"));
            categories.Add(new Category(nextCategoryId++, "Database Systems", "SQL queries, relational models, normalization, and transactions"));
            categories.Add(new Category(nextCategoryId++, "Cybersecurity Basics", "Encryption, authentication, and common security threats"));
            categories.Add(new Category(nextCategoryId++, "Computer Networks", "Protocols, IP addressing, routing, and network layers"));

            admins.Add(new Admin(nextUserId++, "admin", "admin123", "admin@ulster.ac.uk"));
            students.Add(new Student(nextUserId++, "student1", "pass123", "student1@ulster.ac.uk", "active"));

            DateTime d1 = ParseDate("01/09/2025");
            DateTime d2 = ParseDate("07/09/2025");
            DateTime d3 = ParseDate("11/09/2025");
            DateTime d4 = ParseDate("13/09/2025");

            Category prog = categories.First(c => c.CategoryName == "Programming Concepts");
            Category ds = categories.First(c => c.CategoryName == "Data Structures");
            Category sd = categories.First(c => c.CategoryName == "Software Design");
            Category wd = categories.First(c => c.CategoryName == "Web Development");
            Category db = categories.First(c => c.CategoryName == "Database Systems");
            Category cy = categories.First(c => c.CategoryName == "Cybersecurity Basics");
            Category nw = categories.First(c => c.CategoryName == "Computer Networks");

            quizzes.Add(new Quiz(nextQuizId++, "OOP Fundamentals", "Covers basics of object-oriented programming", prog, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Data Structures", "Focuses on arrays, lists, stacks, queues, trees, and their applications.", ds, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Software Design", "Includes design patterns, architecture principles, and system modelling.", sd, new List<Question>(), d1));
            quizzes.Add(new Quiz(nextQuizId++, "Web Development", "HTML, CSS, JavaScript, and client-server interactions", wd, new List<Question>(), d2));
            quizzes.Add(new Quiz(nextQuizId++, "Database Systems", "SQL queries, relational models, normalization, and transactions.", db, new List<Question>(), d2));
            quizzes.Add(new Quiz(nextQuizId++, "Cybersecurity Basics", "Encryption, authentication, and common security threats", cy, new List<Question>(), d3));
            quizzes.Add(new Quiz(nextQuizId++, "Computer Networks", "Protocols, IP addressing, routing, and network layers", nw, new List<Question>(), d4));
        }
        private static DateTime ParseDate(string ddMMyyyy)
        {
            return DateTime.ParseExact(ddMMyyyy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        // =========================
        // Small input helpers
        // =========================
        private static int ReadIntPrompt(string prompt, int min, int max)
        {
            Console.Write(prompt);
            return ReadInt(min, max);
        }

        private static string ReadNonEmptyPrompt(string prompt)
        {
            Console.Write(prompt);
            return ReadNonEmpty();
        }

        private static bool YesNoPrompt(string prompt)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim().ToLower();
            return s == "y" || s == "yes";
        }

        private static string ReadStatusPrompt(string prompt)
        {
            Console.Write(prompt);
            return ReadStatus();
        }

        private static int ReadInt(int min, int max)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (int.TryParse(s, out int v) && v >= min && v <= max) return v;
                Console.Write($"Enter number between {min} and {max}: ");
            }
        }

        private static string ReadNonEmpty()
        {
            while (true)
            {
                string s = (Console.ReadLine() ?? "").Trim();
                if (s.Length > 0) return s;
                Console.Write("Cannot be empty: ");
            }
        }

        private static string ReadStatus()
        {
            while (true)
            {
                string s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                if (s == "active" || s == "inactive") return s;
                Console.Write("Enter active or inactive: ");
            }
        }

        private static void Pause(string msg = "")
        {
            if (!string.IsNullOrWhiteSpace(msg)) Console.WriteLine("\n" + msg);
            Console.WriteLine("\nPress ENTER...");
            Console.ReadLine();
        }

    }
}
