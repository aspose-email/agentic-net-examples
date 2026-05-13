using System;
using System.Text;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MailMessage with UTF‑8 subject characters
            using (MailMessage mailMessage = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Привет мир",          // Subject containing non‑ASCII characters
                "This is the body."))
            {
                // Explicitly set the subject encoding to UTF‑8
                mailMessage.SubjectEncoding = Encoding.UTF8;

                // Define output file path
                string outputPath = "output.eml";

                // Ensure the target directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to the file system
                mailMessage.Save(outputPath);
                Console.WriteLine($"Message saved to '{outputPath}' with UTF‑8 subject encoding.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
