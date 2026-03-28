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
            // Define output MSG file path
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDirectory, "SampleMessage.msg");

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare a simple attachment file
            string attachmentPath = Path.Combine(outputDirectory, "Attachment.txt");
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "This is a sample attachment.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("sender@example.com", "Sender Name");
                mailMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                mailMessage.Subject = "Sample MSG Message";
                mailMessage.Body = "This is the body of the sample MSG message.";
                mailMessage.IsBodyHtml = false;

                // Add attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    mailMessage.Attachments.Add(attachment);

                    // Convert to MAPI message and save as MSG
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        try
                        {
                            mapiMessage.Save(outputPath);
                            Console.WriteLine($"MSG file saved successfully at: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                        }
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
