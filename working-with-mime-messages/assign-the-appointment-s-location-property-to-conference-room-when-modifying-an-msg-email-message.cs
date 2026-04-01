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
            string inputPath = "appointment.msg";
            string outputPath = "appointment_updated.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    placeholderCalendar.Save(inputPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Check if the message is a calendar item
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    // Convert to MapiCalendar
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Update the Location property
                    calendar.Location = "Conference Room";

                    // Get the underlying MapiMessage after modification
                    using (MapiMessage updatedMsg = calendar.GetUnderlyingMessage())
                    {
                        // Save the modified MSG
                        updatedMsg.Save(outputPath);
                    }

                    Console.WriteLine($"Appointment location updated and saved to: {outputPath}");
                }
                else
                {
                    Console.Error.WriteLine("The provided MSG file is not a calendar item.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
