using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client (preserve variable name "client")
            string mailboxUri = "http://exchange.example.com/ews/exchange.asmx";
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
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
                catch (WebException webEx)
                {
                    Console.Error.WriteLine($"Network error while listing messages: {webEx.Message}");
                    if (webEx.Response != null)
                    {
                        Console.Error.WriteLine($"Response: {webEx.Response}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
