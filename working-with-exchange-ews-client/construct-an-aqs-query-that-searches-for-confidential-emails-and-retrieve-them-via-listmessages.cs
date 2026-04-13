using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Create EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create/connect EWS client: {ex.Message}");
                return;
            }

            // Build AQS query for confidential emails
            ExchangeAdvancedSyntaxMailQuery query = new ExchangeAdvancedSyntaxMailQuery("confidential");

            // Retrieve messages from Inbox that match the query
            ExchangeMessageInfoCollection messages = null;
            try
            {
                messages = client.ListMessages("inbox", query);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                return;
            }

            // Display subjects of the retrieved messages
            foreach (ExchangeMessageInfo info in messages)
            {
                Console.WriteLine($"Subject: {info.Subject}");
            }

            // Dispose client
            client.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
