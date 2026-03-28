using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare the initial members collection
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Created Distribution List Id: " + distributionListId);

                // Fetch and display the members to verify creation
                MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Fetched members count: " + fetchedMembers.Count);
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine(address.Address);
                }

                // Add an additional member to the existing distribution list
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("charlie@example.com"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added new member to distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
