using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the mailbox URI and credentials.
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client and set the timeout to 30 seconds (30000 ms).
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                client.Timeout = 30000;

                // Example operation: retrieve mailbox information.
                var mailboxInfo = client.MailboxInfo;
                Console.WriteLine("Mailbox information retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
