using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace EmailAttachmentExtractor
{
    // Author: Aspose.Email example - extracts attachments from a MSG file and saves them to disk.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MSG file path
                string msgFilePath = "input.msg";

                // Verify the MSG file exists
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                    return;
                }

                // Output directory for attachments
                string attachmentsFolder = "Attachments";

                // Ensure the output directory exists
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }

                // Load the Outlook message
                MapiMessage message = MapiMessage.Load(msgFilePath);

                // Iterate through each attachment and save it
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string outputPath = Path.Combine(attachmentsFolder, attachment.FileName);
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
