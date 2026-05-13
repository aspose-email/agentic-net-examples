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
            string inputFilePath = "calendar.msg";
            string outputFilePath = "calendar.ics";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify input file exists; create a placeholder if it does not
            if (!File.Exists(inputFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    var placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));

                    placeholderCalendar.Save(inputFilePath, MapiCalendarSaveOptions.DefaultMsg);
                    Console.WriteLine($"Placeholder MSG created at {inputFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                }

                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Load the MAPI message and export to iCalendar
            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(inputFilePath))
                {
                    var item = mapiMessage.ToMapiMessageItem();

                    if (item is MapiCalendar calendar)
                    {
                        var saveOptions = new MapiCalendarIcsSaveOptions
                        {
                            KeepOriginalDateTimeStamp = true,
                            ProductIdentifier = "Aspose.Email.Sample"
                        };

                        calendar.Save(outputFilePath, saveOptions);
                        Console.WriteLine($"Calendar exported successfully to {outputFilePath}");
                    }
                    else
                    {
                        // Create a minimal placeholder .ics file
                        try
                        {
                            File.WriteAllText(outputFilePath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                            Console.WriteLine($"Input MSG is not a calendar item. Placeholder ICS created at {outputFilePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error writing placeholder ICS: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing calendar: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
