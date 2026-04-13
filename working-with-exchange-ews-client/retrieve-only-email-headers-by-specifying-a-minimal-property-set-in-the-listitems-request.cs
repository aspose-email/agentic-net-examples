using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve the list of message URIs in the Inbox
                string[] itemUris = client.ListItems(inboxUri);

                // Define the minimal property set – only transport message headers
                List<Aspose.Email.Mapi.PropertyDescriptor> headerProperties = new List<Aspose.Email.Mapi.PropertyDescriptor>
                {
                    KnownPropertyList.TransportMessageHeaders
                };

                // Fetch each message with the minimal property set and display headers
                foreach (string itemUri in itemUris)
                {
                    MailMessage message = client.FetchMessage(itemUri, headerProperties);
                    Console.WriteLine($"Subject: {message.Subject}");
                    foreach (string headerKey in message.Headers.Keys)
                    {
                        Console.WriteLine($"{headerKey}: {message.Headers[headerKey]}");
                    }
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
