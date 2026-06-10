using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new calendar item
            MapiCalendar calendar = new MapiCalendar();
            if (string.IsNullOrEmpty(calendar.Body))
            {
                calendar.Body = "Calendar item body";
            }

            calendar.Subject = "Team Meeting";
            calendar.StartDate = DateTime.Now.AddDays(1);
            calendar.EndDate = DateTime.Now.AddDays(1).AddHours(1);

            // Define a category and assign it to the calendar item
            string categoryName = "ProjectX";
            calendar.Categories = new string[] { categoryName };

            // Query the calendar item to confirm the category was stored
            bool categoryExists = calendar.Categories != null && Array.IndexOf(calendar.Categories, categoryName) >= 0;
            if (categoryExists)
            {
                Console.WriteLine($"Category '{categoryName}' successfully added to the calendar item.");
            }
            else
            {
                Console.WriteLine($"Category '{categoryName}' was not found on the calendar item.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
