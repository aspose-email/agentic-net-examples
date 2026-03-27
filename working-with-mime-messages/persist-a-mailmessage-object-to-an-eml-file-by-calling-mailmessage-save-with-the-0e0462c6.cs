using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output EML file path
            string outputPath = "output.eml";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a simple mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Save the message to an EML file using default options
                message.Save(outputPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
