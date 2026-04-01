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
            string outputMsgPath = "output.msg";
            string attachmentPath1 = "attachment1.txt";
            string attachmentPath2 = "attachment2.jpg";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare placeholder attachments if they are missing
            try
            {
                if (!File.Exists(attachmentPath1))
                {
                    File.WriteAllText(attachmentPath1, "Placeholder text file.");
                }

                if (!File.Exists(attachmentPath2))
                {
                    // Create a tiny JPEG placeholder (empty file is acceptable for demo)
                    File.WriteAllBytes(attachmentPath2, new byte[0]);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachment files: {ex.Message}");
                return;
            }

            // Create the mail message
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To = "recipient@example.com";
                mail.Subject = "Sample MSG with Attachments";
                mail.Body = "This is a sample message body.";

                // Add custom MIME header
                mail.Headers["X-Custom-Header"] = "CustomValue";

                // Add attachments
                try
                {
                    mail.Attachments.Add(new Attachment(attachmentPath1));
                    mail.Attachments.Add(new Attachment(attachmentPath2));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachments: {ex.Message}");
                    return;
                }

                // Iterate and display headers using Keys as required
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }

                // Save as MSG with preserved original dates
                try
                {
                    var msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                    {
                        PreserveOriginalDates = true
                    };
                    mail.Save(outputMsgPath, msgSaveOptions);
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
