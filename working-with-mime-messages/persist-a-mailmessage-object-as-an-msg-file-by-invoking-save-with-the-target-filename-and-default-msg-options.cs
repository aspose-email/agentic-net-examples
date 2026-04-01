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
                // Define the output MSG file path
                string outputPath = "output.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create and configure a MailMessage
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com");
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = "Test Message";
                    message.Body = "This is a test email.";

                    // Save the message as MSG using default options
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}
