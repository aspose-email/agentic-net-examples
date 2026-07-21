using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the EML message
            string outputPath = "output.eml";

            // Ensure the target directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a simple email message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("sender@example.com");
                mailMessage.To.Add(new MailAddress("recipient@example.com"));
                mailMessage.Subject = "Test Email";
                mailMessage.Body = "This is a test email.";

                // Persist the message to an EML file using default save options
                mailMessage.Save(outputPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
