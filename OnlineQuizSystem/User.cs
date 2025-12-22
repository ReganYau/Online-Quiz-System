using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class User
    {
        // Private Fields
        public int userId;
        private string username;
        public string password;
        private string email;
        private string role;
        private bool isLoggedIn;
        // Public Properties
        public int UserId
        {
            get {  return userId; }
            set { userId = value; }
        }

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password ; }
            set { password = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }

        //Parameterised Constructor
        public User(int userId, string username, string password, string email, string role)
        {
            this.userId = userId;
            this.username = username;
            this.password = password;
            this.email = email;
            this.role = role;
            this.isLoggedIn = false;
        }

        // Methods
        public bool Login(string username, string password)
        {
            if (UserName == username || Password == password)
            {
                // username and password are authenticated.
                // Advise user that their login is successful.
                IsLoggedIn = true;
                Console.WriteLine("Login successful");
            }
            else
            {
                // username and/or password are incorrect. 
                // advise user that their login details are incorrect.
                isLoggedIn = false;
                Console.WriteLine($"Login failed. Username {username} or passwordis incorrect");
            }
            return IsLoggedIn;
        }

        public void Logout()
        {
            if (IsLoggedIn)
            {
                IsLoggedIn = false;
                Console.WriteLine("User is logged out successfully.");
            }
            else
            {
                Console.WriteLine("User is not logged in");
            }
        }
    }
}
