using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

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

            // Skip real network calls when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    HashSet<string> uniqueSenders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        if (info != null && info.Sender != null)
                        {
                            // Sender is typically a MailAddress; use its string representation
                            string senderAddress = info.Sender.ToString();
                            uniqueSenders.Add(senderAddress);
                        }
                    }

                    Console.WriteLine($"Total unique senders: {uniqueSenders.Count}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
