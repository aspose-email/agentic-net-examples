using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputMsgPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare a sample attachment file
            string attachmentFilePath = "sample.txt";
            if (!File.Exists(attachmentFilePath))
            {
                try
                {
                    File.WriteAllText(attachmentFilePath, "This is a sample attachment.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating attachment file: {ex.Message}");
                    return;
                }
            }

            // Create a new MailMessage and populate its fields
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("sender@example.com", "Sender Name");
                mailMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                mailMessage.Subject = "Sample Email with Attachment";
                mailMessage.Body = "Hello,\n\nThis email contains a sample attachment.\n\nBest regards.";
                mailMessage.IsBodyHtml = false;

                // Add the attachment
                Attachment attachment = new Attachment(attachmentFilePath);
                mailMessage.Attachments.Add(attachment);

                // Set up MSG save options to preserve original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as MSG
                try
                {
                    mailMessage.Save(outputMsgPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
