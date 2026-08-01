using Aspose.Email.Clients.Exchange.Dav;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        // Connection parameters (replace with real values).
        string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Skip external calls when placeholder credentials are used.
        if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        if (string.IsNullOrWhiteSpace(mailboxUri))
        {
            Console.Error.WriteLine("Mailbox URI is not provided.");
            return;
        }

        // Define a filter: unread messages from a specific sender.
        MailQuery query = new MailQuery("(('From' Contains 'sender@example.com') & 'Seen' = 'False')");

        try
        {
            // Instantiate ExchangeClient.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Retrieve messages from the Inbox that satisfy the filter.
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query.ToString());

                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Received: {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while accessing Exchange: {ex.Message}");
        }
    }
}
