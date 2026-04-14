using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                foreach (ExchangeDistributionList dl in distributionLists)
                {
                    // Fetch members of the current distribution list
                    MailAddressCollection members = client.FetchDistributionList(dl);

                    // Check if the list has more than 50 members
                    if (members != null && members.Count > 50)
                    {
                        // Log the identifier (using DisplayName as the identifier)
                        Console.WriteLine($"Distribution List: {dl.DisplayName} has {members.Count} members.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
