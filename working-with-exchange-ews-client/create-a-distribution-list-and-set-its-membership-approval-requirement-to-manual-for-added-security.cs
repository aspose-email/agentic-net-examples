using Aspose.Email.Clients.Exchange;
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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare the distribution list
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Sample Distribution List"
                };

                // Prepare members (add at least one member to satisfy creation)
                MailAddressCollection members = new MailAddressCollection
                {
                    new MailAddress("member1@example.com")
                };

                // Create the distribution list on the server
                string dlId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine($"Distribution List created with Id: {dlId}");

                // Note: Membership approval requirement is manual by default.
                // If a specific property were available, it would be set here.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
