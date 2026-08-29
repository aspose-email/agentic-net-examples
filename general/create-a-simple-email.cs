using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "SimpleEmail.eml");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Hello", "This is a simple email."))
            {
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Email saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
