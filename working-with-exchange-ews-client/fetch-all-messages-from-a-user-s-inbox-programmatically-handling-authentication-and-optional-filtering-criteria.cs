using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping connection.");
                return;
            }

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List all messages in the default Inbox folder
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages();

                    foreach (ExchangeMessageInfo info in messageInfos)
                    {
                        // Output basic information
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received: {info.InternalDate}");
                        Console.WriteLine($"URI: {info.UniqueUri}");
                        Console.WriteLine(new string('-', 40));

                        // Optionally fetch the full message if needed
                        using (MailMessage fullMessage = client.FetchMessage(info.UniqueUri))
                        {
                            // Example: display the first 100 characters of the body
                            string bodySnippet = fullMessage.Body != null && fullMessage.Body.Length > 100
                                ? fullMessage.Body.Substring(0, 100) + "..."
                                : fullMessage.Body;
                            Console.WriteLine($"Body snippet: {bodySnippet}");
                            Console.WriteLine(new string('=', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message retrieval: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
