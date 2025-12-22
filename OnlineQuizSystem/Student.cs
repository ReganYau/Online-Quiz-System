using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class Student : User 
    {
        // Private Fields
        private string status;
        private List<Feedback> feedbacks;
        private List<Results> resultsHistory;

        // Public Properties
        public string Status
        { 
            get { return status; } 
            set { status = value; } 
        }

        public List<Feedback> Feedbacks
        {
            get { return feedbacks;}
            set { feedbacks = value; }
        }

        public List<Results> ResultsHistory
        {
            get { return resultsHistory;}
            set { resultsHistory = value; }
        }

        // Parameterised Constructor
        public Student (int id, string username, string password, string email, string role, string status)
            : base(id, username, password, email, role)
        {
            this.status = status;
            this.feedbacks = new List<Feedback>();
            this.resultsHistory = new List<Results>();
        }

        // Methods
        public void PlayQuiz()
        {

        }

        public void SortByCategory()
        {

        }

        public void ExitApp()
        {
            Console.WriteLine("Exiting application");
        }
    }
}
