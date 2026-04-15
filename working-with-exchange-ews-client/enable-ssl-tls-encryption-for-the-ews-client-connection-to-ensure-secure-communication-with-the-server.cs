using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI must use HTTPS to enforce SSL/TLS encryption
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (implements IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Perform a simple operation to verify the connection
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
