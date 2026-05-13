using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "International Characters Test";

                // Set body with international characters
                mailMessage.Body = "Привет мир! こんにちは世界!";

                // Replace charset with UTF-8
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.PreferredTextEncoding = Encoding.UTF8;

                // Define output path
                string outputPath = Path.Combine(Environment.CurrentDirectory, "output.eml");
                string outputDir = Path.GetDirectoryName(outputPath);

                // Ensure the output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the message to file (EML format)
                try
                {
                    mailMessage.Save(outputPath);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
