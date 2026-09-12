using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define output MSG file path
                string outputPath = "OutputMessage.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create a new MailMessage instance
                MailMessage message = new MailMessage();

                // Set basic properties
                message.From = "sender@example.com";
                message.Subject = "Sample Message with Recipients";
                message.Body = "This message demonstrates adding To, CC, and BCC recipients.";

                // Populate To recipients
                MailAddressCollection toCollection = message.To;
                toCollection.Add("to1@example.com");
                toCollection.Add("to2@example.com");

                // Populate CC recipients (property name is CC)
                MailAddressCollection ccCollection = message.CC;
                ccCollection.Add("cc1@example.com");
                ccCollection.Add("cc2@example.com");

                // Populate BCC recipients
                MailAddressCollection bccCollection = message.Bcc;
                bccCollection.Add("bcc1@example.com");
                bccCollection.Add("bcc2@example.com");

                // Save the message as an MSG file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message saved successfully to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                }
                finally
                {
                    // Dispose the MailMessage to release resources
                    message.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
