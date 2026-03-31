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
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping external Exchange call.");
                return;
            }

            // Create the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Define the date range for filtering
                    DateTime startDate = new DateTime(2023, 1, 1);
                    DateTime endDate   = new DateTime(2023, 12, 31);

                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    // Iterate over the messages and apply the date range filter
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        DateTime messageDate = info.InternalDate;
                        if (messageDate >= startDate && messageDate <= endDate)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                            Console.WriteLine($"Date   : {messageDate}");
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
