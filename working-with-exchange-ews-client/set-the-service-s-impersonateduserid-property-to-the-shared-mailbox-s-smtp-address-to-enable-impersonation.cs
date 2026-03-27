using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the EWS endpoint and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Set impersonation to the shared mailbox's SMTP address
                    client.ImpersonateUser(ItemChoice.SmtpAddress, "shared@example.com");
                    Console.WriteLine("Impersonation set to shared mailbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Impersonation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}