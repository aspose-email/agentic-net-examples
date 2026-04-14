using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the email file (EML format)
            string emlPath = "sample.eml";

            // Guard: ensure the file exists before attempting to load
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the email message safely
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            using (message)
            {
                // Extract plain‑text body from an HTML email
                string plainTextBody;
                if (message.IsBodyHtml)
                {
                    // GetHtmlBodyText parses the HTML and returns plain text
                    plainTextBody = message.GetHtmlBodyText(true);
                }
                else
                {
                    plainTextBody = message.Body ?? string.Empty;
                }

                // Simulate storing the plain‑text body into a database column
                // (Here we just output it to the console)
                Console.WriteLine("Extracted plain‑text body:");
                Console.WriteLine(plainTextBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
