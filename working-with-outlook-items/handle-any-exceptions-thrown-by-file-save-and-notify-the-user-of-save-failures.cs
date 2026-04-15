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
            // Placeholder credentials and URI – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create and dispose the Exchange client safely.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Placeholder message URI – replace with a valid URI.
                string messageUri = "/Inbox/Message1.eml";

                MailMessage message = null;
                try
                {
                    // Fetch the message from the server.
                    message = client.FetchMessage(messageUri);
                }
                catch (Exception fetchEx)
                {
                    Console.Error.WriteLine($"Failed to fetch message: {fetchEx.Message}");
                    return;
                }

                // Ensure the fetched message is disposed.
                using (message)
                {
                    // Define the output file path.
                    string outputPath = "SavedMessage.eml";

                    // Ensure the output directory exists.
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }
                        catch (Exception dirEx)
                        {
                            Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                            return;
                        }
                    }

                    // Attempt to save the message and handle any save errors.
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message successfully saved to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message to file: {saveEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
