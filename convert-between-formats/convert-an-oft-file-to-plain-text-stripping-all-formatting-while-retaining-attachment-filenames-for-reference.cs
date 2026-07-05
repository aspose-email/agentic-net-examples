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
            // Author note: Simple console utility to convert an OFT template to plain text.
            string inputPath = "template.oft";
            string outputPath = "output.txt";

            // Verify input file exists
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

            // Load the OFT file as a MapiMessage
            MapiMessage oftMessage = MapiMessage.Load(inputPath);

            // Write plain text content and attachment filenames to the output file
            using (StreamWriter writer = new StreamWriter(outputPath, false))
            {
                writer.WriteLine("Subject: " + oftMessage.Subject);
                writer.WriteLine("From: " + oftMessage.SenderName);
                writer.WriteLine();
                writer.WriteLine("Body:");
                writer.WriteLine(oftMessage.Body ?? string.Empty);
                writer.WriteLine();
                writer.WriteLine("Attachments:");
                foreach (MapiAttachment attachment in oftMessage.Attachments)
                {
                    writer.WriteLine("- " + attachment.FileName);
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
