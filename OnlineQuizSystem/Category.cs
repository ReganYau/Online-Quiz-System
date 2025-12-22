using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    public class Category
    {
        // Private Fields
        private int categoryId;
        private string categoryName;
        private string categoryDescription;

        // Public Properties
        public int CategoryId 
        {
            get { return categoryId; } 
            private set { categoryId = value; } 
        }
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public string CategoryDescription
        {
            get { return categoryDescription; }
            set { categoryDescription = value; }
        }

        // Parameterised Constructor
        public Category(int categoryId, string categoryName, string categoryDescription)
        {
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.categoryDescription = categoryDescription;
        }
                
    }
}
