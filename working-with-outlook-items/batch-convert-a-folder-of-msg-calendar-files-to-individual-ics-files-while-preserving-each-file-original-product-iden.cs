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
            // Input folder containing MSG calendar files
            string inputFolder = "InputMsgFolder";
            // Output folder for generated ICS files
            string outputFolder = "OutputIcsFolder";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            // Process each MSG file in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files in {inputFolder}: {ex.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                try
                {
                    // Load the MSG file as a MapiMessage
                    using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                    {
                        // Determine output file path
                        string icsFileName = Path.GetFileNameWithoutExtension(msgFilePath) + ".ics";
                        string icsFilePath = Path.Combine(outputFolder, icsFileName);

                        // Check if the message is a calendar item
                        if (msg.SupportedType == MapiItemType.Calendar)
                        {
                            // Convert to MapiCalendar
                            MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                            // Ensure required fields are set
                            if (string.IsNullOrEmpty(calendar.Subject))
                                calendar.Subject = !string.IsNullOrEmpty(msg.Subject) ? msg.Subject : "No Subject";

                            if (string.IsNullOrEmpty(calendar.Body))
                                calendar.Body = !string.IsNullOrEmpty(msg.Body) ? msg.Body : "No Description";

                            // Prepare save options preserving the original product identifier
                            MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions
                            {
                                ProductIdentifier = Path.GetFileNameWithoutExtension(msgFilePath)
                            };

                            // Save as ICS
                            calendar.Save(icsFilePath, saveOptions);
                        }
                        else
                        {
                            // Not a calendar item – create a minimal placeholder ICS file
                            using (StreamWriter writer = new StreamWriter(icsFilePath))
                            {
                                writer.WriteLine("BEGIN:VCALENDAR");
                                writer.WriteLine("END:VCALENDAR");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
