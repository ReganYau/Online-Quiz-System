using System;

namespace OnlineQuizSystem
{
    // Base class used to represent a user account (admin or student).
    public class User
    {
        public int UserId { get; } // Storing unique user ID.
        public string UserName { get; private set; } // Storing username.
        public string Password { get; private set; } // Storing password.
        public string Email { get; private set; } // Storing email.
        public string Role { get; protected set; } // Storing role (admin/student).

        public bool IsLoggedIn { get; private set; } // Storing login state.

        // Constructor used to create a user object.
        public User(int userId, string username, string password, string email, string role)
        {
            UserId = userId; // Populating user ID.
            UserName = username ?? ""; // Populating username.
            Password = password ?? ""; // Populating password.
            Email = email ?? ""; // Populating email.
            Role = role ?? "user"; // Populating role (defaulting to user).
        }

        // Method used to log in a user by checking username and password.
        public virtual bool Login(string username, string password)
        {
            // Checking username and password values.
            if (!CheckUsername(username) || !CheckPassword(password))
                return false; // Returning false if authentication failed.

            IsLoggedIn = true; // Setting login state to true.
            return true; // Returning true if login succeeded.
        }

        // Method used to log out the user.
        public virtual void Logout() => IsLoggedIn = false; // Setting login state to false.

        // Method used to check if given username matches stored username.
        public bool CheckUsername(string username)
            => string.Equals(UserName, username ?? "", StringComparison.OrdinalIgnoreCase); // Returning username comparison reult.

        // Method used to check if given password matches stored password.
        public bool CheckPassword(string password)
            => Password == (password ?? ""); // Returning password comparison result.

        // Method used to update user profile values if new ones are provided.
        public void UpdateProfile(string? username, string? email, string? password)
        {
            if (!string.IsNullOrWhiteSpace(username)) UserName = username.Trim(); // Updating username if provided.
            if (!string.IsNullOrWhiteSpace(email)) Email = email.Trim(); // Updating email if provided.
            if (!string.IsNullOrWhiteSpace(password)) Password = password; // Updating password if provided.
        }

        // Method used to return a readable string for the user.
        public override string ToString()
            => $"{UserId} | {UserName} | {Email} | {Role}";
    }
}
