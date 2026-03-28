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
            // Define paths for the attachment and the output MSG file
            string attachmentPath = "attachment.txt";
            string outputMsgPath = "output.msg";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            try
            {
                if (!File.Exists(attachmentPath))
                {
                    // Create a simple text file as a placeholder attachment
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error preparing attachment file: {ex.Message}");
                return;
            }

            // Ensure the directory for the output MSG file exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error preparing output directory: {ex.Message}");
                return;
            }

            // Create a MailMessage and add the attachment
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Sample Email with Attachment";
                mailMessage.Body = "This email contains an attachment. The attachment will be persisted when saved as MSG.";

                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    mailMessage.Attachments.Add(attachment);
                }

                // Convert the MailMessage to a MapiMessage (MSG format)
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    try
                    {
                        // Save the MapiMessage as an MSG file; attachments are automatically persisted
                        mapiMessage.Save(outputMsgPath);
                        Console.WriteLine($"MSG file saved successfully to '{outputMsgPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
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
