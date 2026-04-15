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
            string messagePath = "message.eml";
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {messagePath}");
                return;
            }

            // Load the email message from the file
            using (MailMessage mailMessage = MailMessage.Load(messagePath))
            {
                // Convert to MapiMessage to work with attachments
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    string outputDirectory = "AudioAttachments";
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Iterate through all attachments and save MP3 files
                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        string fileName = attachment.FileName;
                        if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            string outputPath = Path.Combine(outputDirectory, fileName);
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved audio attachment: {outputPath}");
                        }
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
