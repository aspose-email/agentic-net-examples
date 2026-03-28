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
            // Initialize EWS client with credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Retrieve mailbox information
                var mailboxInfo = client.MailboxInfo;

                // Display diagnostic information
                Console.WriteLine("Mailbox Info: " + mailboxInfo);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
