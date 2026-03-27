using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange Web Services endpoint and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Define a new distribution list
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Initial members
                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("user1@example.com"));
                initialMembers.Add(new MailAddress("user2@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, initialMembers);
                distributionList.Id = distributionListId; // Set the returned Id for further operations
                Console.WriteLine("Created Distribution List Id: " + distributionListId);

                // Add an additional member
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("user3@example.com"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added a new member to the Distribution List.");

                // Fetch and display current members
                MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Current Distribution List members:");
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine(address.Address);
                }

                // Delete the distribution list permanently
                client.DeleteDistributionList(distributionList, true);
                Console.WriteLine("Deleted the Distribution List.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}