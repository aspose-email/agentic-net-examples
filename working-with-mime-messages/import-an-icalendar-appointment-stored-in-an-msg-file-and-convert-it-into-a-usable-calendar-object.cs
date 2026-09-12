using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            // Path to the source MSG file containing the iCalendar appointment
            string msgPath = "calendar.msg";

            // Verify that the input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(msgPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            try
            {
                // Load the MSG file
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    // Ensure the MSG contains a calendar item
                    if (msg.SupportedType == MapiItemType.Calendar)
                    {
                        // Convert the MAPI message to a MapiCalendar object
                        MapiCalendar mapiCalendar = (MapiCalendar)msg.ToMapiMessageItem();

                        // Display some calendar details
                        Console.WriteLine("Subject: " + mapiCalendar.Subject);
                        Console.WriteLine("Start:   " + mapiCalendar.StartDate);
                        Console.WriteLine("End:     " + mapiCalendar.EndDate);

                        // Optional: save the calendar as an iCalendar (ICS) file
                        string icsPath = Path.Combine("output", "appointment.ics");
                        string outputDir = Path.GetDirectoryName(icsPath);
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Use the default iCalendar save options
                        mapiCalendar.Save(icsPath, MapiCalendarSaveOptions.DefaultIcs);
                        Console.WriteLine($"Calendar saved to: {icsPath}");
                    }
                    else
                    {
                        Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
