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
            // Input MSG file path (adjust as needed)
            string msgFilePath = "sample.msg";

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

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Display basic properties
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Prepare attachment output folder
                string attachmentFolder = "Attachments";
                if (!Directory.Exists(attachmentFolder))
                {
                    Directory.CreateDirectory(attachmentFolder);
                }

                // Iterate over attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentFolder, attachment.FileName ?? "unnamed.dat");
                    Console.WriteLine("Attachment: " + attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine("Saved to: " + attachmentPath);
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
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
