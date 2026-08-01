using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    // Author: Aspose.Email .NET example
    class Program
    {
        static void Main()
        {
            try
            {
                // ----- Configuration -----
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string domain = ""; // optional, can be empty

                // Simple placeholder check – skip real connection if dummy data is used
                if (username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // ----- Create EWS client -----
                IEWSClient client;
                try
                {
                    client = EWSClient.GetEWSClient(mailboxUri, username, password, domain);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                    return;
                }

                // Ensure the client is disposed properly
                using (client)
                {
                    // ----- Get Drafts folder URI -----
                    string draftsUri = client.MailboxInfo.DraftsUri;
                    if (string.IsNullOrEmpty(draftsUri))
                    {
                        Console.Error.WriteLine("Unable to retrieve Drafts folder URI.");
                        return;
                    }

                    // ----- Create a simple MAPI message -----
                    using (MapiMessage mapiMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is a sample email body."))
                    {
                        // Append the message as a draft
                        string itemId;
                        try
                        {
                            itemId = client.AppendMessage(draftsUri, mapiMessage, false);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to append message: {ex.Message}");
                            return;
                        }

                        Console.WriteLine($"Message appended to Drafts. Item ID: {itemId}");
                    }

                    // ----- List messages in Drafts folder -----
                    try
                    {
                        ExchangeMessageInfoCollection messages = client.ListMessages(draftsUri);
                        Console.WriteLine($"Drafts folder contains {messages.Count} message(s).");
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine($"- Subject: {info.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
