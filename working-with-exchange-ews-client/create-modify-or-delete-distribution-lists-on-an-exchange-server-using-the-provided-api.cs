using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // -------------------- Create a Distribution List --------------------
                ExchangeDistributionList newList = new ExchangeDistributionList();
                newList.DisplayName = "Sample Distribution List";

                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("alice@example.com"));
                initialMembers.Add(new MailAddress("bob@example.com"));

                string distributionListId = client.CreateDistributionList(newList, initialMembers);
                Console.WriteLine($"Created Distribution List with Id: {distributionListId}");

                // -------------------- Add Members to the Distribution List --------------------
                MailAddressCollection membersToAdd = new MailAddressCollection();
                membersToAdd.Add(new MailAddress("carol@example.com"));
                client.AddToDistributionList(newList, membersToAdd);
                Console.WriteLine("Added new members to the Distribution List.");

                // -------------------- Fetch Members of the Distribution List --------------------
                MailAddressCollection currentMembers = client.FetchDistributionList(newList);
                Console.WriteLine("Current members in the Distribution List:");
                foreach (MailAddress address in currentMembers)
                {
                    Console.WriteLine(address.Address);
                }

                // -------------------- Delete Specific Members from the Distribution List --------------------
                MailAddressCollection membersToDelete = new MailAddressCollection();
                membersToDelete.Add(new MailAddress("bob@example.com"));
                client.DeleteFromDistributionList(newList, membersToDelete);
                Console.WriteLine("Deleted specified members from the Distribution List.");

                // -------------------- List All Private Distribution Lists --------------------
                ExchangeDistributionList[] allLists = client.ListDistributionLists();
                Console.WriteLine("All private Distribution Lists:");
                foreach (ExchangeDistributionList list in allLists)
                {
                    Console.WriteLine($"{list.DisplayName} (Id: {list.Id})");
                }

                // -------------------- Delete the Distribution List --------------------
                client.DeleteDistributionList(newList, true);
                Console.WriteLine("Deleted the Distribution List permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
