using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // EWS service endpoint and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Retrieve existing private distribution lists
                ExchangeDistributionList[] lists = client.ListDistributionLists();
                if (lists == null || lists.Length == 0)
                {
                    Console.WriteLine("No distribution lists found.");
                    return;
                }

                // For demonstration, use the first distribution list
                ExchangeDistributionList dl = lists[0];
                Console.WriteLine($"Updating Distribution List: {dl.DisplayName} (Id: {dl.Id})");

                // Prepare members to add
                MailAddressCollection newMembers = new MailAddressCollection();
                newMembers.Add(new MailAddress("newmember1@example.com"));
                newMembers.Add(new MailAddress("newmember2@example.com"));

                // Append members to the distribution list
                client.AddToDistributionList(dl, newMembers);
                Console.WriteLine("Members added successfully.");

                // Fetch and display updated members
                MailAddressCollection updatedMembers = client.FetchDistributionList(dl);
                Console.WriteLine("Current members of the distribution list:");
                foreach (MailAddress address in updatedMembers)
                {
                    Console.WriteLine($"- {address.Address}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}