namespace OnlineQuizSystem
{
    // Class used to represent a quiz category.
    public class Category
    {
        public int CategoryID { get; } // Storing the unique category ID.
        public string CategoryName { get; set; } // Storing the category name.
        public string CategoryDescription { get; set; } // Storing the category description.

        // Constructor used to create a category.
        public Category(int id, string name, string description)
        {
            CategoryID = id; // Populating category ID.
            CategoryName = name ?? ""; // Populating category name - defaults to null.
            CategoryDescription = description ?? ""; // Populating category description with a default to nul.
        }

        // Method used to return a readable string for the category.
        public override string ToString() => $"{CategoryID}. {CategoryName}";
    }
}
