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
            // Paths for the attachment and the output MSG file
            string attachmentPath = "sample.txt";
            string outputPath = "output.msg";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating attachment file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            // Create a MailMessage with basic fields
            using (MailMessage mailMessage = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Subject",
                "This is the body of the email."))
            {
                // Add an attachment to the MailMessage
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    mailMessage.Attachments.Add(attachment);
                }

                // Convert the MailMessage to a MapiMessage (MSG format)
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Save the MapiMessage as an MSG file
                    try
                    {
                        mapiMessage.Save(outputPath);
                        Console.WriteLine($"MSG file saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
