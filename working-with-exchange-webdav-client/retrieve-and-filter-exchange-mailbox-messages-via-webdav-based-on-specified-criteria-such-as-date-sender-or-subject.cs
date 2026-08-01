using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace ExchangeWebDavSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server URL (WebDAV endpoint) and credentials
                string mailboxUri = "https://exchange.example.com/Exchange";
                string username = "username";
                string password = "password";

                // Skip external calls when placeholder values are detected
                if (mailboxUri.Contains("example.com") || username.Contains("username") || password.Contains("password"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Instantiate ExchangeClient using the required pattern
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Folder to search (e.g., Inbox)
                    string folder = "Inbox";

                    // DASL query to filter messages by sender, subject and date
                    string query = "From='sender@example.com' AND Subject='Test' AND SentDate >= '2023-01-01'";

                    // Retrieve the filtered messages
                    ExchangeMessageInfoCollection messages = client.ListMessages(folder, query);

                    // Output basic information for each message
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From   : {messageInfo.From}");
                        Console.WriteLine($"Sent   : {messageInfo.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
