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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create credentials object
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the WebDAV client inside a using block to ensure disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, credentials))
            {
                // List messages in the Inbox folder
                string inboxFolder = "Inbox";
                try
                {
                    // Retrieve message URIs from the specified folder
                    var messages = client.ListMessages(inboxFolder);
                    Console.WriteLine($"Messages in folder '{inboxFolder}':");
                    foreach (var messageUri in messages)
                    {
                        Console.WriteLine(messageUri);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
