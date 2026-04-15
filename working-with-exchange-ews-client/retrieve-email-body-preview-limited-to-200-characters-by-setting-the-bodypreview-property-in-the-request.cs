using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
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
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Get Inbox folder URI
            string inboxUri = client.MailboxInfo.InboxUri;

            // List messages in the Inbox
            ExchangeMessageInfoCollection messages = null;
            try
            {
                messages = client.ListMessages(inboxUri);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                return;
            }

            if (messages == null || messages.Count == 0)
            {
                Console.WriteLine("No messages found in the Inbox.");
                return;
            }

            // Take the first message URI
            string messageUri = messages[0].UniqueUri;

            // Fetch the message with the preview property
            MailMessage mailMessage = null;
            try
            {
                List<Aspose.Email.Mapi.PropertyDescriptor> extendedProps = new List<Aspose.Email.Mapi.PropertyDescriptor> { KnownPropertyList.Preview };
                mailMessage = client.FetchMessage(messageUri, extendedProps);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to fetch message: {ex.Message}");
                return;
            }

            // Retrieve preview (fallback to body if preview not available)
            string previewText = string.Empty;
            if (!string.IsNullOrEmpty(mailMessage.Body))
            {
                previewText = mailMessage.Body.Length > 200
                    ? mailMessage.Body.Substring(0, 200)
                    : mailMessage.Body;
            }

            Console.WriteLine("Email preview (max 200 chars):");
            Console.WriteLine(previewText);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
