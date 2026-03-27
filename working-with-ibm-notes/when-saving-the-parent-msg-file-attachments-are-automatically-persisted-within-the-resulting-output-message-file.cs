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
            // Define paths
            string attachmentPath = "attachment.txt";
            string outputMsgPath = "output.msg";

            // Ensure attachment file exists; create a placeholder if missing
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
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
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

            // Create a MailMessage and add the attachment
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Test Message with Attachment";
                mailMessage.Body = "This is the body of the email.";

                try
                {
                    mailMessage.Attachments.Add(new Attachment(attachmentPath));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding attachment: {ex.Message}");
                    return;
                }

                // Convert MailMessage to MapiMessage (attachments are preserved)
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    try
                    {
                        mapiMessage.Save(outputMsgPath);
                        Console.WriteLine($"Message saved successfully to '{outputMsgPath}'.");
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
