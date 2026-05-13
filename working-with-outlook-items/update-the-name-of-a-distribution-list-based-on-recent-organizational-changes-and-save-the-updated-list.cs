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
            // Placeholder credentials and server URI
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // ID of the distribution list to be renamed (placeholder)
            string oldDistributionListId = "old-list-id";

            // New display name for the distribution list
            string newDisplayName = "New Distribution List Name";

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serverUri, new NetworkCredential(username, password)))
            {
                // Prepare the existing distribution list object with its Id
                ExchangeDistributionList oldList = new ExchangeDistributionList
                {
                    Id = oldDistributionListId
                };

                // Fetch current members of the distribution list
                MailAddressCollection members = client.FetchDistributionList(oldList);

                // Delete the old distribution list permanently
                client.DeleteDistributionList(oldList, true);

                // Create a new distribution list with the new display name and same members
                ExchangeDistributionList newList = new ExchangeDistributionList
                {
                    DisplayName = newDisplayName
                };

                string newListId = client.CreateDistributionList(newList, members);
                Console.WriteLine($"Distribution list renamed. New Id: {newListId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
