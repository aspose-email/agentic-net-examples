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
            // Placeholder values – replace with real server and credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sharedMailboxAddress = "shared@example.com";

            // Guard against placeholder credentials to avoid real network calls in CI.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve information about the shared mailbox.
                ExchangeMailboxInfo sharedMailboxInfo = client.GetMailboxInfo(sharedMailboxAddress);

                // Prepare a simple email message addressed from the shared mailbox.
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sharedMailboxAddress);
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test email from shared mailbox";
                message.Body = "This email was sent using a shared mailbox address.";

                // Send the message using the client.
                client.Send(message);
                Console.WriteLine("Message sent successfully from shared mailbox.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
