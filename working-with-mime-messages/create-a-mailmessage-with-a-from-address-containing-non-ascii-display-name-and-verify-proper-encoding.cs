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
            // Output file path
            string outputPath = "NonAsciiFrom.eml";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MailAddress with a non‑ASCII display name using UTF‑8 encoding
            MailAddress fromAddress = new MailAddress("sender@example.com", "Jürgen Müller", Encoding.UTF8);

            // Build the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = fromAddress;
                message.To.Add("recipient@example.com");
                message.Subject = "Test non‑ASCII From";
                message.Body = "This is a test message.";

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }

                // Load the saved message and verify the From display name encoding
                try
                {
                    using (MailMessage loaded = MailMessage.Load(outputPath))
                    {
                        string loadedDisplayName = loaded.From.DisplayName;
                        Console.WriteLine($"Loaded From display name: {loadedDisplayName}");
                        bool isCorrect = loadedDisplayName == "Jürgen Müller";
                        Console.WriteLine($"Encoding verification: {(isCorrect ? "Success" : "Failure")}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
