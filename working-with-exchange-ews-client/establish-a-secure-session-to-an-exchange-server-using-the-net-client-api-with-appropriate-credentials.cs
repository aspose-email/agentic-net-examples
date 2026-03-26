using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange Web Services (EWS) endpoint and user credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create a NetworkCredential instance
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client (implements IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Access mailbox information to verify the connection
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine("Connected to Exchange server successfully.");
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error communicating with Exchange server: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}