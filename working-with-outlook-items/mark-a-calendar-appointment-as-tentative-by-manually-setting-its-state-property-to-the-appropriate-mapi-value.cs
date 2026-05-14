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
            // Define output file path
            string outputPath = Path.Combine("Output", "TentativeAppointment.msg");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MapiCalendar (appointment)
            using (MapiCalendar calendar = new MapiCalendar())
            {
                // Set basic appointment details
                calendar.Subject = "Project Discussion";
                calendar.Location = "Conference Room";
                calendar.StartDate = new DateTime(2024, 12, 1, 10, 0, 0);
                calendar.EndDate = new DateTime(2024, 12, 1, 11, 0, 0);
                calendar.Body = "Discuss project milestones and deliverables.";

                // Mark the appointment as tentative by setting the busy status
                calendar.BusyStatus = MapiCalendarBusyStatus.Tentative;

                // Save the appointment to a MSG file using default MSG save options
                calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
            }

            Console.WriteLine("Tentative appointment saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
