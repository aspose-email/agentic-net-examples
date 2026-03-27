using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace DistributionListSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize the EWS client (replace placeholders with real values)
                NetworkCredential credentials = new NetworkCredential("username", "password");
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Create a new distribution list
                    ExchangeDistributionList distributionList = new ExchangeDistributionList
                    {
                        DisplayName = "Sample Distribution List"
                    };

                    MailAddressCollection initialMembers = new MailAddressCollection();
                    initialMembers.Add(new MailAddress("alice@example.com"));
                    initialMembers.Add(new MailAddress("bob@example.com"));

                    string listId = client.CreateDistributionList(distributionList, initialMembers);
                    Console.WriteLine($"Created distribution list with Id: {listId}");

                    // Set the Id on the distribution list object for further operations
                    distributionList.Id = listId;

                    // Fetch and display current members
                    MailAddressCollection currentMembers = client.FetchDistributionList(distributionList);
                    Console.WriteLine("Current members:");
                    foreach (MailAddress address in currentMembers)
                    {
                        Console.WriteLine($"- {address.Address}");
                    }

                    // Add additional members
                    MailAddressCollection membersToAdd = new MailAddressCollection();
                    membersToAdd.Add(new MailAddress("carol@example.com"));
                    client.AddToDistributionList(distributionList, membersToAdd);
                    Console.WriteLine("Added new member: carol@example.com");

                    // Delete a member
                    MailAddressCollection membersToDelete = new MailAddressCollection();
                    membersToDelete.Add(new MailAddress("bob@example.com"));
                    client.DeleteFromDistributionList(distributionList, membersToDelete);
                    Console.WriteLine("Deleted member: bob@example.com");

                    // Fetch and display updated members
                    MailAddressCollection updatedMembers = client.FetchDistributionList(distributionList);
                    Console.WriteLine("Updated members:");
                    foreach (MailAddress address in updatedMembers)
                    {
                        Console.WriteLine($"- {address.Address}");
                    }

                    // Delete the distribution list permanently
                    client.DeleteDistributionList(distributionList, true);
                    Console.WriteLine("Distribution list deleted permanently.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
