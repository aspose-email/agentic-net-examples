using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Message";
                message.Body = "This is a sample email saved as MSG.";

                // Save the message as MSG using default options
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }

            Console.WriteLine($"Message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
