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
            // Placeholder server/credentials – replace with real values when available.
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (serverUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server call.");
                return;
            }

            // Initialize the WebDAV Exchange client.
            using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
            {
                // The Message-ID header value to filter by.
                string targetMessageId = "<desired-message-id@example.com>";

                // Retrieve messages from the Inbox that match the specified Message-ID.
                ExchangeMessageInfoCollection messages = client.ListMessagesById("Inbox", targetMessageId);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full mail message using its unique URI.
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
