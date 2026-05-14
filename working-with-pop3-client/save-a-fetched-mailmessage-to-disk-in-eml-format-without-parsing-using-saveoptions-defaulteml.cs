using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder server and credentials
            string mailboxUri = "https://example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Initialize Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // URI of the message to fetch (placeholder)
                string messageUri = "/mailfolders/Inbox/messages/12345";

                // Destination file path
                string outputPath = "fetchedMessage.eml";

                // Ensure the target directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                    // Fetch the message as a MailMessage object
                    MailMessage mailMessage = client.FetchMessage(messageUri);

                    // Save the message in EML format without additional parsing
                    mailMessage.Save(outputPath, SaveOptions.DefaultEml);

                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching or saving message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
