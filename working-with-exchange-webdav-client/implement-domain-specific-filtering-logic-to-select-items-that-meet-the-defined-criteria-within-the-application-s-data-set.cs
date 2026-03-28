using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange WebDAV client
            try
            {
                using (ExchangeClient client = new ExchangeClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection infoCollection = client.ListMessages("Inbox");

                    // Convert the collection to a list of MailMessage objects
                    List<MailMessage> messages = new List<MailMessage>();
                    foreach (ExchangeMessageInfo info in infoCollection)
                    {
                        // Fetch each message by its unique URI
                        using (MailMessage fetched = client.FetchMessage(info.UniqueUri))
                        {
                            // Clone the message to detach it from the using scope
                            messages.Add(fetched.Clone());
                        }
                    }

                    // Example usage: output subject of each message
                    foreach (MailMessage message in messages)
                    {
                        Console.WriteLine(message.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error initializing or using Exchange client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
