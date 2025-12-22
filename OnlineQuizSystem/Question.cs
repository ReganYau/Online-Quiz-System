using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class Question
    {
        // Private Fields
        private int questionId;
        private string questionText;
        private List<string> questionOptions;
        private string questionCorrectAnswer;
        private string questionDifficultyLevel;

        // Public Properties
        public int QuestionId
        { 
            get { return questionId; } 
            set { questionId = value; } 
        }

        public string QuestionText
        { 
            get { return questionText; } 
            set { questionText = value; } 
        }

        public List<string> QuestionOptions
        {
            get { return questionOptions; }
            set { questionOptions = value; }
        }

        public string QuestionCorrectAnswer
        {
            get { return questionCorrectAnswer; }
            set { questionCorrectAnswer = value; }
        }

        public string QuestionDificultyLevel
        {
            get { return questionDifficultyLevel; }
            set { questionDifficultyLevel = value; }
        }

        // Parameterised Constructor
        public Question (int questionId, string questionText, List<string> questionOptions, string questionCorrectAnswer, string questionDifficultyLevel)
        {
            this.questionId = questionId;
            this.questionText = questionText;
            this.questionOptions = questionOptions ?? new List<string>();
            this.questionCorrectAnswer = questionCorrectAnswer;
            this.questionDifficultyLevel = questionDifficultyLevel;
        }

        public bool RightAnswer()
        {

        }

        public bool WrongAnswer()
        {
        }

        public int QuestionNumber()
        {

        }

        public void SubmitAnswers()
        {

        }
    }
}
