using Aspose.Email.Calendar;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output file paths
            string inputPath = "calendar.msg";
            string outputPath = "calendar_updated.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a minimal calendar message
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder Calendar", "Placeholder body"))
                    {
                        // Set the message class to indicate a calendar item
                        placeholder.MessageClass = "IPM.Appointment";

                        // Ensure the directory for the input file exists
                        string inputDir = Path.GetDirectoryName(inputPath);
                        if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                        {
                            Directory.CreateDirectory(inputDir);
                        }

                        // Save the placeholder message
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Load the message from file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Verify that the loaded message is a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(outputPath, ".ics");
                    try
                    {
                        File.WriteAllText(placeholderIcsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                        Console.WriteLine($"Input MSG is not a calendar item. Placeholder ICS created at {placeholderIcsPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing placeholder ICS: {ex.Message}");
                    }
                    return;

                    Console.Error.WriteLine("The provided file is not a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    // Access the underlying MapiMessage to add a custom property
                    MapiMessage underlyingMessage = calendar.GetUnderlyingMessage();

                    // Define the custom property value (project code)
                    string projectCode = "PRJ123";
                    byte[] projectCodeBytes = Encoding.Unicode.GetBytes(projectCode);

                    // Add the custom property named "ProjectCode"
                    underlyingMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, projectCodeBytes, "ProjectCode");

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the updated calendar as a MSG file using default MSG save options
                    calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
