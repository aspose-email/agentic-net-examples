using Aspose.Email.Mime;
using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Email with Custom Header";
                message.Body = "This email contains a custom X-Project header.";

                // Add a custom X-Project header
                // HeaderCollection supports Add(string name, string value)
                message.Headers.Add("X-Project", "Project-12345");

                // Verify that the header was added
                Console.WriteLine("Custom header added:");
                Console.WriteLine($"X-Project: {message.Headers["X-Project"]}");

                // Optionally, save the message to a file (guarded by file existence check)
                string outputPath = "sample.eml";
                try
                {
                    // Ensure the directory exists
                    string directory = System.IO.Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }

                    // Save the message
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
