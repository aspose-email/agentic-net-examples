using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all distribution lists (mail‑enabled groups)
                ExchangeDistributionList[] groups = client.ListDistributionLists();

                Console.WriteLine("Mail‑enabled security groups:");
                foreach (ExchangeDistributionList group in groups)
                {
                    Console.WriteLine($"- {group.DisplayName} (Id: {group.Id})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
