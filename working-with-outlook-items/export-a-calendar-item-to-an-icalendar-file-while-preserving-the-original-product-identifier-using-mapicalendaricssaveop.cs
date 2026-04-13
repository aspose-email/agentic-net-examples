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
            string outputPath = "ExportedCalendar.ics";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create a sample MapiCalendar object
            MapiCalendar mapiCalendar = new MapiCalendar(
                location: "Conference Room",
                summary: "Project Kickoff",
                description: "Discuss project goals and timelines.",
                startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                endDate: new DateTime(2024, 5, 20, 11, 0, 0));

            // Configure save options to preserve the original product identifier
            MapiCalendarIcsSaveOptions icsSaveOptions = new MapiCalendarIcsSaveOptions
            {
                ProductIdentifier = "MyCompany Calendar Exporter"
            };

            // Save the calendar to an iCalendar file
            try
            {
                using (mapiCalendar)
                {
                    mapiCalendar.Save(outputPath, icsSaveOptions);
                }
                Console.WriteLine($"Calendar exported successfully to '{outputPath}'.");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save calendar: {saveEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
