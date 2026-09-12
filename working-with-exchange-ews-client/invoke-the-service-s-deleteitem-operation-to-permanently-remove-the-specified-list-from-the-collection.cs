using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace with actual Exchange service URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = "example.com";

            // Identifier of the distribution list to delete.
            // This should be the Id property value obtained from ListDistributionLists or other means.
            string distributionListId = "AAMkAD..."; // placeholder Id

            try
            {
                // Create the EWS client. The client implements IDisposable, so we use a using block.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password, domain))
                {
                    // Prepare the distribution list object with the known Id.
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.Id = distributionListId;

                    // Delete the distribution list permanently.
                    client.DeleteDistributionList(distributionList, true);
                }

                Console.WriteLine("Distribution list deleted successfully.");
            }
            catch (Exception ex)
            {
                // Output any errors without throwing.
                Console.Error.WriteLine($"Error deleting distribution list: {ex.Message}");
            }
        }
    }
}
