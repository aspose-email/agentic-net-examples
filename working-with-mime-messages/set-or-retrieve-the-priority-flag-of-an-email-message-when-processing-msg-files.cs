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
            // Input and output file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Guard input file existence
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapiMsg = MapiMessage.Load(inputPath);

            // Convert to MailMessage to work with the Priority property
            MailConversionOptions conversionOpts = new MailConversionOptions();
            MailMessage mailMsg = mapiMsg.ToMailMessage(conversionOpts);

            // Retrieve current priority
            MailPriority currentPriority = mailMsg.Priority;
            Console.WriteLine($"Current priority: {currentPriority}");

            // Set a new priority (e.g., High)
            mailMsg.Priority = MailPriority.High;
            Console.WriteLine("Priority set to High.");

            // Save the modified message back to MSG format
            mailMsg.Save(outputPath);
            Console.WriteLine($"Modified message saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
