using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder mailbox URI and credentials
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = "EXAMPLE";

            // Guard against executing real network calls with placeholder data
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder mailbox URI detected. Skipping network call.");
                return;
            }

            // Create a NetworkCredential instance with required details
            NetworkCredential credentials = new NetworkCredential(username, password, domain);

            // Initialize the EWS client using the credentials
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optionally assign the credentials explicitly (already set via factory)
                client.Credentials = credentials;

                // Example operation: fetch mailbox information (wrapped in its own try/catch)
                try
                {
                    var mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine("Mailbox URI: " + mailboxInfo.MailboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during mailbox operation: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
