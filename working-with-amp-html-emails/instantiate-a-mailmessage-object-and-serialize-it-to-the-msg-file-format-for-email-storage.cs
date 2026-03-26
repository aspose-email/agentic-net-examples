using System;
using System.IO;
using Aspose.Email;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the output MSG file path
                string outputPath = "sample.msg";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create a MailMessage instance
                using (Aspose.Email.MailMessage message = new Aspose.Email.MailMessage())
                {
                    message.From = new Aspose.Email.MailAddress("sender@example.com");
                    message.To.Add(new Aspose.Email.MailAddress("recipient@example.com"));
                    message.Subject = "Test Message";
                    message.Body = "This is a test email saved as MSG.";

                    // Prepare save options for MSG format
                    Aspose.Email.MsgSaveOptions saveOptions = new Aspose.Email.MsgSaveOptions(Aspose.Email.MailMessageSaveType.OutlookMessageFormat);

                    // Save the message to the specified file
                    try
                    {
                        message.Save(outputPath, saveOptions);
                        Console.WriteLine("Message saved to " + outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to save message: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}