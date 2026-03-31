using System;
using System.IO;
using Aspose.Email;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Target file path for the EML file
                string outputPath = "output.eml";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create a simple mail message
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("sender@example.com");
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));
                    mailMessage.Subject = "Test Email";
                    mailMessage.Body = "This is a test email saved as EML.";

                    // Save the message using default EML options
                    mailMessage.Save(outputPath, SaveOptions.DefaultEml);
                }

                Console.WriteLine($"MailMessage saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
