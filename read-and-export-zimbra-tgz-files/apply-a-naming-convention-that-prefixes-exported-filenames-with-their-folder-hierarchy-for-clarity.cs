using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid runtime errors in CI.
            if (string.IsNullOrEmpty(mailboxUri) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Skipping Exchange operation due to placeholder credentials.");
                return;
            }

            // Create and use the Exchange client inside a using block to ensure disposal.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages from the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    // Iterate over the collection and output each message subject.
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine(messageInfo.Subject);
                    }
                }
                catch (Exception ex)
                {
                    // Handle connection or operation errors gracefully.
                    Console.Error.WriteLine("Error while accessing Exchange server: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
