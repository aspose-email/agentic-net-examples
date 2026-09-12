using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace DistributionListSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace the placeholders with actual Exchange service URL and credentials.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard: skip external calls when placeholders are still in use.
                if (serviceUrl.Contains("example.com") ||
                    username.Equals("user@example.com", StringComparison.OrdinalIgnoreCase) ||
                    password.Equals("password", StringComparison.Ordinal))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                    return;
                }

                // Create and connect the EWS client.
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Prepare a new distribution list definition.
                    ExchangeDistributionList distributionList = new ExchangeDistributionList
                    {
                        DisplayName = "Sample Distribution List"
                    };

                    // Define the members of the distribution list.
                    MailAddressCollection members = new MailAddressCollection
                    {
                        new MailAddress("member1@example.com"),
                        new MailAddress("member2@example.com")
                    };

                    // Create the distribution list on the Exchange server.
                    string listId = ewsClient.CreateDistributionList(distributionList, members);
                    Console.WriteLine("Created distribution list with Id: " + listId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
