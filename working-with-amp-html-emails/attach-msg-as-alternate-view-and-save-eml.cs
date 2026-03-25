using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgFilePath = "input.msg";

            // Verify input file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMsg = MapiMessage.Load(msgFilePath))
            {
                // Save the MapiMessage to a memory stream in MSG format
                using (MemoryStream msgStream = new MemoryStream())
                {
                    mapiMsg.Save(msgStream, SaveOptions.DefaultMsg);
                    msgStream.Position = 0;

                    // Create an AlternateView from the MSG stream
                    ContentType msgContentType = new ContentType("application/vnd.ms-outlook");
                    AlternateView altView = new AlternateView(msgStream, msgContentType);

                    // Create a simple MailMessage
                    using (MailMessage mailMsg = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the plain text body."))
                    {
                        // Add the MSG AlternateView
                        mailMsg.AlternateViews.Add(altView);

                        // Save the resulting message as EML
                        string outputPath = Path.Combine(outputDir, "result.eml");
                        mailMsg.Save(outputPath, SaveOptions.DefaultEml);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}