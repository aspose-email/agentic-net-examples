using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgPath = "sample.msg";

                // Ensure the directory for the MSG file exists
                string msgDir = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(msgDir))
                {
                    Directory.CreateDirectory(msgDir);
                }

                // Verify the file exists before attempting to load
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

                // Load the Outlook message
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Display basic properties
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderName}");
                Console.WriteLine($"Body: {msg.Body}");

                // Prepare attachments directory
                string attachmentsDir = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
                Directory.CreateDirectory(attachmentsDir);

                // Process attachments, if any
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}");

                    // Save each attachment to the attachments directory
                    string attachmentPath = Path.Combine(attachmentsDir, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment to: {attachmentPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception handling
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
