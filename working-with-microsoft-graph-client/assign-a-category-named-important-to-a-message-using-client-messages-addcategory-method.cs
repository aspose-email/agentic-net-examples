using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple MAPI message
            MapiMessage message = new MapiMessage(
                "from@example.com",
                "to@example.com",
                "Sample Subject",
                "This is the body of the message.");

            // Assign the "Important" category to the message
            FollowUpManager.AddCategory(message, "Important");

            // Display the categories assigned to the message
            var categories = FollowUpManager.GetCategories(message);
            Console.WriteLine("Categories assigned to the message:");
            foreach (string cat in categories)
            {
                Console.WriteLine("- " + cat);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
