using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network calls
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client (required variable name)
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Prepare a draft mail message
                using (MailMessage message = new MailMessage())
                {
                    message.From = defaultEmail;
                    message.To.Add("recipient@example.com");
                    message.Subject = "Sample Draft";
                    message.Body = "This is a draft message created with Aspose.Email.";

                    // Add custom headers
                    message.Headers["X-Custom-Header"] = "CustomValue";
                    message.Headers["X-Another-Header"] = "AnotherValue";

                    // Define output path
                    string outputPath = "draft.eml";

                    try
                    {
                        // Ensure directory exists
                        string directory = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the draft locally
                        message.Save(outputPath);
                        Console.WriteLine($"Draft saved to: {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
