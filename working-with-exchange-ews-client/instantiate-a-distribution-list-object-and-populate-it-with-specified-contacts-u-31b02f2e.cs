using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare members collection
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                distributionList.Id = distributionListId;
                Console.WriteLine("Distribution List created with Id: " + distributionListId);

                // Add an additional member
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("charlie@example.com"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added additional member to the distribution list.");

                // Fetch members to verify
                MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Members of the distribution list:");
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine("- " + address.Address);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}