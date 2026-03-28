using System;
using System.Net;
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
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Identify the distribution list to be removed (replace with actual Id)
                string distributionListId = "DL_ID";

                // Prepare the distribution list object with the known Id
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.Id = distributionListId;

                // Delete the distribution list permanently
                client.DeleteDistributionList(distributionList, true);

                Console.WriteLine("Distribution list deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
