using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare initial members
                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("alice@example.com"));
                initialMembers.Add(new MailAddress("bob@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, initialMembers);
                Console.WriteLine($"Created Distribution List ID: {distributionListId}");

                // Fetch and display current members
                MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Current members:");
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine($"- {address.Address}");
                }

                // Add additional members
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("carol@example.com"));
                additionalMembers.Add(new MailAddress("dave@example.com"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added additional members.");

                // Fetch and display updated members
                MailAddressCollection updatedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Updated members:");
                foreach (MailAddress address in updatedMembers)
                {
                    Console.WriteLine($"- {address.Address}");
                }

                // List all private distribution lists
                ExchangeDistributionList[] allLists = client.ListDistributionLists();
                Console.WriteLine("All private distribution lists:");
                foreach (ExchangeDistributionList list in allLists)
                {
                    Console.WriteLine($"- {list.DisplayName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
