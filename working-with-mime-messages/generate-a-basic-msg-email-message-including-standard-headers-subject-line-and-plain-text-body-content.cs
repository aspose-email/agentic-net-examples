using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.msg");
            string outputDir = Path.GetDirectoryName(outputPath);

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a MailMessage with standard fields
            using (MailMessage mail = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the plain text body of the message."))
            {
                // Add a custom header
                mail.Headers.Add("X-Custom-Header", "CustomValue");

                // Iterate headers using Keys as required
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }

                // Save the message as MSG
                try
                {
                    mail.Save(outputPath);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
