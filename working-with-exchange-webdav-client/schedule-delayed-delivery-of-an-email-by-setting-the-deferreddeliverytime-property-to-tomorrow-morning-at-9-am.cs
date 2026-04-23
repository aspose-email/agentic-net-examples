using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection info – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Build a simple mail message.
                MailMessage mail = new MailMessage();
                mail.From = "sender@example.com";
                mail.To.Add("recipient@example.com");
                mail.Subject = "Delayed Delivery Test";
                mail.Body = "This email is scheduled to be delivered tomorrow at 9 AM.";

                // Convert to MAPI message to set the DeferredDeliveryTime property.
                MapiMessage mapiMsg = MapiMessage.FromMailMessage(mail);

                // Set the deferred delivery time to tomorrow 9:00 AM (UTC).
                DateTime deferredTime = DateTime.UtcNow.Date.AddDays(1).AddHours(9);
                mapiMsg.SetProperty(KnownPropertyList.DeferredDeliveryTime, deferredTime);

                // Append the message to the Drafts folder; Exchange will handle delayed delivery.
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                client.AppendMessage(draftsFolderUri, mapiMsg.ToMailMessage(new MailConversionOptions()));

                Console.WriteLine("Message scheduled for delayed delivery at {0} UTC.", deferredTime);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
