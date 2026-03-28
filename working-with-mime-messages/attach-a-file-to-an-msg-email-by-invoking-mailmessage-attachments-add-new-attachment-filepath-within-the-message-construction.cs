using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string attachmentPath = "sample.txt";
            string outputMsgPath = "output.msg";

            // Ensure attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create the email message and add the attachment
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Email with Attachment";
                message.Body = "Please see the attached file.";

                // Add attachment
                message.Attachments.Add(new Attachment(attachmentPath));

                // Save the message as MSG
                try
                {
                    message.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputMsgPath}");
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
