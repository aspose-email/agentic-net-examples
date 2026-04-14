using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Unique identifier of the private distribution list
                string distributionListId = "unique-id-here";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Prepare the distribution list object with the identifier
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.Id = distributionListId;

                // Fetch the members of the distribution list
                MailAddressCollection members = client.FetchDistributionList(distributionList);

                Console.WriteLine("Distribution List Members:");
                foreach (MailAddress address in members)
                {
                    Console.WriteLine(address.Address);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
