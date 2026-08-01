using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output MSG file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load existing MSG or create a minimal placeholder
            if (File.Exists(inputPath))
            {
                using (MailMessage mailMessage = MailMessage.Load(inputPath))
                {
                    // Add a new recipient
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));

                    // Save the modified message as MSG
                    mailMessage.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
            else
            {
                // Create a new message as a placeholder
                using (MailMessage mailMessage = new MailMessage("sender@example.com", "original@example.com"))
                {
                    mailMessage.Subject = "Placeholder";
                    mailMessage.Body = "This is a placeholder message.";

                    // Add the desired recipient
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));

                    // Save the new message as MSG
                    mailMessage.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
