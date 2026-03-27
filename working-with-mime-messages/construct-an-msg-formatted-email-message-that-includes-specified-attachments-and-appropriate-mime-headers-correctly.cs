using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define paths for the output MSG file and an attachment.
            string outputMsgPath = "output.msg";
            string attachmentPath = "example.txt";

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure the attachment file exists; create a minimal placeholder if missing.
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

            // Create a MailMessage, set basic fields, add a custom header and the attachment.
            using (MailMessage mail = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the email."))
            {
                mail.Headers.Add("X-Custom-Header", "CustomValue");
                mail.Attachments.Add(new Attachment(attachmentPath));

                // Convert the MailMessage to a MapiMessage.
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail))
                {
                    // Save the MapiMessage as an MSG file.
                    try
                    {
                        mapiMessage.Save(outputMsgPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                        return;
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
