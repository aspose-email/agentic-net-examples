using System;
using System.IO;
using Aspose.Email;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare output directory and file path
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "sample.msg");

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create a new mail message and add recipients
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com", "Sender Name");
                    message.To.Add(new MailAddress("recipient@example.com"));
                    
                    // Add CC recipients (with and without display name)
                    message.CC.Add(new MailAddress("cc1@example.com"));
                    message.CC.Add(new MailAddress("cc2@example.com", "CC Two"));

                    message.Subject = "Test Email with CC";
                    message.Body = "This email demonstrates adding CC recipients.";

                    // Save the message using MsgSaveOptions as required
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    message.Save(outputPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
