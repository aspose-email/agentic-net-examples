using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attachment files (create placeholders if missing)
            string[] attachmentPaths = { "attachment1.txt", "attachment2.jpg" };
            foreach (string path in attachmentPaths)
            {
                try
                {
                    string dir = Path.GetDirectoryName(Path.GetFullPath(path));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (!File.Exists(path))
                    {
                        // Create a minimal placeholder file
                        File.WriteAllText(path, "Placeholder content");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare attachment '{path}': {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Message with Attachments";
                message.Body = "Please see the attached files.";

                // Add attachments
                foreach (string path in attachmentPaths)
                {
                    try
                    {
                        message.Attachments.Add(new Attachment(path));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment '{path}': {ex.Message}");
                        return;
                    }
                }

                // Ensure output directory exists
                string outputPath = "output.msg";
                try
                {
                    string outDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Save the message as MSG
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
