using System;
using System.Collections.Generic;
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
            LoadSampleData();
            MainMenu();
        }
        // Main Menu
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();

                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Student");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminMenu();
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
        public static void AdminMenu()
        {
            Console.Clear();
            PrintHeader();

            Console.WriteLine("Admin Login (Authenticatiion Required)\n");
            Console.Write("Admin ID: ");
            int id = ReadInt(1, int.MaxValue);

            Console.Write("Password: ");
            string pw = ReadNonEmpty();

            Admin admin = admins.FirstOrDefault(a => a.UserId == id);

            if (admin == null || admin.Login(admin.UserName, pw) == false)
            {
                Console.WriteLine("\nAccess denied. Press enter to return.");
                Console.ReadLine();
                return;
            }
            admin.LoginDate = DateTime.Now;

            while (true)
            {
                Console.Clear();
                PrintHeader();
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
                        ManageQuestionsMenu();
                        break;
                    case "2":
                        ManageUsersMenu();
                        break;
                    case "3":
                        ManageCategoriesMenu();
                        break;
                    case "4":
                        SaveQuestionsToCsv("questions.csv");
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

        private static void ManageQuestionsMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine("Manage Questions\n");
(
                Quiz quiz = SelectQuiz();
                if (Quiz == null)
                    return;

                Console.Clear();
                PrintHeader();
                Console.WriteLine($"Quiz: {quiz.QuizTitle} ({quiz.QuizCategory.CategoryName})\n");

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
                        ShowQuestions(quiz);
                        break;
                    case "2":
                        AddQuestionToQuiz(quiz);
                        break;
                    case "3":
                        UpdateQuestionInQuiz(quiz);
                        break;
                    case "4":
                        RemoveQuestionFromQuiz(quiz);
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

        private static void ManageUsersMenu()
        {
            while  (true)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine("Manage Users\n");

                Console.WriteLine("1. Show all users");
                Console.WriteLine("2. Add student");
                Console.WriteLine("3. Remove student");
                Console.WriteLine("4. Update student (username/email/password/status)");
                Console.WriteLine("0. Back");
                Console.Write("\nChoose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllUsers();
                        break;
                    case "2":
                        AddStudent();
                        break;
                    case "3":
                        RemoveStudent();
                        break;
                    case "4":
                        UpdateStudent();
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

        private static void ManageCategoriesMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();
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
                        ShowCategories();
                        break;
                    case "2":
                        AddCategory();
                        break;
                    case "3":
                        UpdateCategory();
                        break;
                    case "4":
                        RemoveCategory();
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
                PrintHeader();

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

    }
}
