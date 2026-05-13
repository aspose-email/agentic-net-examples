using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output path for the email file
            string outputPath = Path.Combine(Environment.CurrentDirectory, "LocalizedEmail.eml");
            string outputDir = Path.GetDirectoryName(outputPath);

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new mail message and set basic properties
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Email with Language Header";
                message.Body = "This email includes a custom X-Language header for localization.";

                // Add custom X-Language header (e.g., en-US)
                message.Headers.Add("X-Language", "en-US");

                // Save the message to an .eml file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Email saved successfully to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save email: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
