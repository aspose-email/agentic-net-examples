using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client (WebDAV). Replace with actual server details.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages from the default Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages();

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Display a brief overview of each message.
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Date: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
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
