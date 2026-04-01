using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder server URI and credentials.
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // If placeholders are detected, skip the external call to avoid runtime failures.
            if (serverUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server call.");
                return;
            }

            // Create the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                // Unique identifiers (message IDs) of the messages to retrieve.
                string[] messageIds = { "AAMkAGI2AAAAAA...", "AAMkAGI3BBBBBB..." };

                foreach (string id in messageIds)
                {
                    // Retrieve message info by ID from the Inbox folder.
                    ExchangeMessageInfoCollection infos = client.ListMessagesById("Inbox", id);

                    // The collection should contain a single item; fetch the full message.
                    foreach (var info in infos)
                    {
                        // Fetch the complete MailMessage using the message's unique URI.
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
