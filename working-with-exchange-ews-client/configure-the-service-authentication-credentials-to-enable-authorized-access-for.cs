using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and user credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client with the credentials
            using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                Console.WriteLine("EWS client authenticated successfully.");
                // Subsequent API operations can be performed using 'client' here.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}