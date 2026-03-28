using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace AsposeEmailEwsExtendedProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values as needed)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and connect the EWS client safely
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                    {
                        // List messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found in the Inbox.");
                            return;
                        }

                        // Take the first message for demonstration
                        ExchangeMessageInfo firstInfo = messages[0];

                        // Define the extended properties we want to retrieve
                        List<PropertyDescriptor> extendedProperties = new List<PropertyDescriptor>
                        {
                            KnownPropertyList.EmsAbSendEmailMessage,
                            KnownPropertyList.EmsAbExtensionName
                        };

                        // Fetch the full message together with the extended properties
                        MailMessage fetchedMessage = client.FetchMessage(firstInfo.UniqueUri, extendedProperties);

                        // Display standard properties
                        Console.WriteLine("Subject: " + fetchedMessage.Subject);
                        Console.WriteLine("From: " + fetchedMessage.From);
                        Console.WriteLine("Date: " + fetchedMessage.Date);

                        // Display extended properties (available in the Headers collection)
                        Console.WriteLine("\nExtended Properties (Headers):");
                        foreach (string headerKey in fetchedMessage.Headers.AllKeys)
                        {
                            Console.WriteLine($"{headerKey}: {fetchedMessage.Headers[headerKey]}");
                        }

                        // -------------------------------------------------
                        // Manipulate an extended property by adding a custom header
                        // -------------------------------------------------
                        MailMessage newMessage = new MailMessage();
                        newMessage.From = new MailAddress("sender@example.com");
                        newMessage.To.Add(new MailAddress("recipient@example.com"));
                        newMessage.Subject = "Test Message with Custom Extended Property";
                        newMessage.Body = "This message includes a custom extended property.";

                        // Add a custom header that will be treated as an extended property
                        newMessage.Headers.Add("X-Custom-Extended-Property", "CustomValue");

                        // Append the new message to the Drafts folder
                        client.AppendMessage(client.MailboxInfo.DraftsUri, newMessage);
                        Console.WriteLine("\nNew message with custom extended property appended to Drafts.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS client operation failed: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
