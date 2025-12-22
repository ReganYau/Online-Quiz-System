using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class Admin : User
    {
        // Private Fields
        private DateTime loginDate;

        // Public Properties
        public DateTime LoginDate
        {
            get { return loginDate; }
            set { loginDate = value; }

        }

        // Parameterised Constructor
        public Admin(int id, string username, string password, string email, string role, DateTime loginDate)
            : base(id, username, password, email, role)
        {
            this.loginDate = loginDate;
        }

        // Methods
        public void AddQuestion()
        {

        }

        public void RemoveQuestion()
        {

        }

        public void UpdateQuestion()
        {

        }

        public void AddUser()
        {

        }

        public void RemoveUser()
        {

        }

        public void UpdateUser()
        {

        }


        public void AddCategory()
        {

        }

        public void RemoveCategory()
        {

        }

        public void UpdateCategory()
        {

        }

        public void SaveQuiz()
        {

        }
    }
}
