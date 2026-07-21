using System;
using System.IO;
using Aspose.Email;

namespace EmailHeaderExample
{
    // Author: Generated example for inserting a custom header and saving as MSG
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output file path
                string outputPath = "output.msg";

                // Ensure the target directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create a new email message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Test Email with Custom Header";
                message.Body = "This is the body of the email.";

                // Insert a user‑defined header
                message.Headers.Add("X-Custom-Header", "MyHeaderValue");

                // Prepare MSG save options (preserve original dates)
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as MSG
                try
                {
                    message.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
