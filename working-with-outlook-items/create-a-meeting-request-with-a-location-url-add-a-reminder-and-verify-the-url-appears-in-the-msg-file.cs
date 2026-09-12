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
            // Output MSG file path
            string outputPath = "MeetingRequest.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a meeting request with a location URL
            using (MapiCalendar calendar = new MapiCalendar(
                "https://example.com/meeting",          // location (URL)
                "Team Sync",                            // summary
                "Discuss project updates",              // description
                DateTime.Now.AddHours(1),               // start time
                DateTime.Now.AddHours(2)))              // end time
            {
                // Add a reminder (15 minutes before start)
                calendar.ReminderSet = true;
                calendar.ReminderDelta = 15; // minutes

                // Save the meeting request as MSG
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    MapiCalendarSaveOptions saveOptions = MapiCalendarSaveOptions.DefaultMsg;
                    calendar.Save(fs, saveOptions);
                }
            }

            // Verify that the location URL appears in the saved MSG file
            using (MapiMessage loadedMsg = MapiMessage.Load(outputPath))
            {
                if (loadedMsg.SupportedType == MapiItemType.Calendar)
                {
                    MapiCalendar loadedCal = (MapiCalendar)loadedMsg.ToMapiMessageItem();
                    if (!string.IsNullOrEmpty(loadedCal.Location) &&
                        loadedCal.Location.Contains("https://example.com/meeting"))
                    {
                        Console.WriteLine("Location URL verified in MSG file.");
                    }
                    else
                    {
                        Console.WriteLine("Location URL not found in MSG file.");
                    }
                }
                else
                {
                    Console.WriteLine("The MSG file does not contain a calendar item.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
