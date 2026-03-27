using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                Console.WriteLine("Connected to Exchange server.");
                Console.WriteLine("Mailbox URI: " + client.MailboxInfo.MailboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
