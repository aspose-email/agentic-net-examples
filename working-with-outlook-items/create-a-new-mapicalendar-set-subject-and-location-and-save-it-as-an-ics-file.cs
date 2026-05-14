using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the output file path for the iCalendar file
            string outputPath = "calendar.ics";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MapiCalendar instance and set its properties
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.Subject = "Team Meeting";
                calendar.Location = "Conference Room";

                // Set start and end times for the appointment
                calendar.StartDate = DateTime.Now.AddHours(1);
                calendar.EndDate = DateTime.Now.AddHours(2);

                // Save the calendar as an iCalendar (ICS) file
                calendar.Save(outputPath);
            }

            Console.WriteLine($"Calendar saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
