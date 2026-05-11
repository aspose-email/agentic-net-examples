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
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Message URI to fetch (replace with a real message URI)
                    string messageUri = "https://exchange.example.com/EWS/MessageId";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageUri.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Fetch the message; extended properties are available via Headers collection
                    MailMessage message = client.FetchMessage(messageUri);

                    Console.WriteLine("Extended properties of the message:");
                    foreach (string headerName in message.Headers.Keys)
                    {
                        string headerValue = message.Headers[headerName];
                        Console.WriteLine($"{headerName}: {headerValue}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
