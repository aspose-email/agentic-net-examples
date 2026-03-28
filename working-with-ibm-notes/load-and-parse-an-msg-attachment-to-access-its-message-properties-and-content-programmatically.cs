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
            // Path to the MSG file to be loaded
            string msgFilePath = "message.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Access basic message properties
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderEmailAddress}");
                Console.WriteLine($"Body: {msg.Body}");

                // Ensure the output directory for attachments exists
                string attachmentsDir = "attachments";
                if (!Directory.Exists(attachmentsDir))
                {
                    Directory.CreateDirectory(attachmentsDir);
                }

                // Iterate through attachments and save each one
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}");
                    string outputPath = Path.Combine(attachmentsDir, attachment.FileName);
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
