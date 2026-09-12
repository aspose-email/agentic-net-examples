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
            // Input MSG file containing the meeting request
            const string inputPath = "meeting.msg";
            // Desired output iCalendar file
            const string outputPath = "meeting.ics";

            // Verify the input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapMsg = MapiMessage.Load(inputPath);

            // Convert to MailMessage (MailMessage implements IDisposable)
            using (MailMessage mailMsg = mapMsg.ToMailMessage(new MailConversionOptions()))
            {
                // Save as iCalendar (.ics). The library infers the format from the extension.
                mailMsg.Save(outputPath);
            }

            Console.WriteLine($"Meeting request successfully saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
