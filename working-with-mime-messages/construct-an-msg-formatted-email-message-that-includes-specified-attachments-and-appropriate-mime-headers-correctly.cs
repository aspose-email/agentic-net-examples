using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputMsgPath = "AddAttachments.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attachment files (create minimal placeholders if missing)
            string[] attachmentFiles = { "1.txt", "1.jpg", "1.doc", "1.rar", "1.pdf" };
            foreach (string file in attachmentFiles)
            {
                if (!File.Exists(file))
                {
                    try
                    {
                        // Create a tiny placeholder file appropriate to its extension
                        File.WriteAllText(file, $"Placeholder content for {file}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder for '{file}': {ex.Message}");
                        return;
                    }
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@from.com";
                message.To = "receiver@to.com";
                message.Subject = "This is message";
                message.Body = "This is body";

                // Add a custom MIME header
                message.Headers.Add("X-Custom-Header", "CustomValue");

                // Add attachments
                foreach (string file in attachmentFiles)
                {
                    Attachment attachment = new Attachment(file);
                    message.AddAttachment(attachment);
                }

                // Save the message as MSG
                try
                {
                    message.Save(outputMsgPath);
                    Console.WriteLine($"Message saved successfully to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
