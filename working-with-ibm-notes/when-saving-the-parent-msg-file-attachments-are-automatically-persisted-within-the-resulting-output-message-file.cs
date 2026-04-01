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
            string attachmentPath = "sample.txt";
            string outputMsgPath = "output.msg";

            // Ensure the attachment file exists; create a minimal placeholder if missing
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

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!Directory.Exists(outputDir))
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

            // Create a MailMessage and add the attachment
            using (MailMessage mail = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the email."))
            {
                // Add attachment
                mail.Attachments.Add(new Attachment(attachmentPath));

                // Convert MailMessage to MapiMessage (attachments are automatically included)
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail))
                {
                    // Save the MapiMessage as an Outlook MSG file
                    mapiMessage.Save(outputMsgPath);
                }
            }

            Console.WriteLine($"MSG file saved to '{outputMsgPath}' with attachments persisted.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
