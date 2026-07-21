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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the Outlook MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderName}");

                // Prepare output directory for extracted files
                string outputDir = "Attachments";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Extract all attachments (including embedded objects)
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Build a safe file name
                    string safeFileName = string.IsNullOrEmpty(attachment.FileName) ? "unnamed_attachment" : attachment.FileName;
                    string outputPath = Path.Combine(outputDir, safeFileName);

                    // Save the attachment to disk
                    attachment.Save(outputPath);
                    Console.WriteLine($"Saved attachment: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
