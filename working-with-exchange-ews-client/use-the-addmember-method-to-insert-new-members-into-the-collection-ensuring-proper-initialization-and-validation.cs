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
            // Define connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Initialize a new distribution list
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare initial members
                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("user1@example.com"));
                initialMembers.Add(new MailAddress("user2@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, initialMembers);
                Console.WriteLine("Created distribution list ID: " + distributionListId);

                // Set the Id so that further operations can target this list
                distributionList.Id = distributionListId;

                // Prepare additional members to add
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("user3@example.com"));
                additionalMembers.Add(new MailAddress("user4@example.com"));

                // Add new members to the existing distribution list
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added additional members to the distribution list.");

                // Retrieve and display all members of the distribution list
                MailAddressCollection allMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Current members of the distribution list:");
                foreach (MailAddress address in allMembers)
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