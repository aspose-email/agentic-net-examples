using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Replace with your actual EWS endpoint and credentials
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // IEWSClient is wrapped in a using statement for automatic disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Client connection safety guard
                try
                {
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // ExchangeMailboxInfo does not have EmailAddress; use MailboxUri or other available properties
                    Console.WriteLine("Mailbox URI: " + mailboxInfo.MailboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to retrieve mailbox info: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
