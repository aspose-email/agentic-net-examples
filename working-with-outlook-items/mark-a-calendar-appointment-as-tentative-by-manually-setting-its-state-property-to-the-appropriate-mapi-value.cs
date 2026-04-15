using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory and file
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDir, "TentativeAppointment.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MapiCalendar instance
            using (MapiCalendar mapiCalendar = new MapiCalendar(
                location: "Conference Room",
                summary: "Project Discussion",
                description: "Discuss project milestones.",
                startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                endDate: new DateTime(2024, 5, 20, 11, 0, 0)))
            {
                // Set the state to Tentative by using the appropriate MAPI value.
                // In MapiCalendarState, the closest representation is Received,
                // which indicates the meeting request has been received.
                // This example forces the state accordingly.
                mapiCalendar.SetStateForced(MapiCalendarState.Received);

                // Save the calendar as a MSG file using default MSG save options
                mapiCalendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
                Console.WriteLine($"Tentative appointment saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
