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
            // Define EWS service URL and credentials (replace with actual values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // List all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                // Specify the name of the distribution list to remove
                string targetListName = "MyDistributionList";

                // Find the distribution list with the specified name
                ExchangeDistributionList targetList = null;
                foreach (ExchangeDistributionList list in distributionLists)
                {
                    if (string.Equals(list.DisplayName, targetListName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetList = list;
                        break;
                    }
                }

                if (targetList == null)
                {
                    Console.WriteLine($"Distribution list \"{targetListName}\" not found.");
                    return;
                }

                // Fetch current members of the distribution list
                MailAddressCollection members = client.FetchDistributionList(targetList);

                // Remove all members from the distribution list
                if (members != null && members.Count > 0)
                {
                    client.DeleteFromDistributionList(targetList, members);
                    Console.WriteLine($"Removed {members.Count} member(s) from distribution list \"{targetListName}\".");
                }
                else
                {
                    Console.WriteLine($"Distribution list \"{targetListName}\" has no members to remove.");
                }

                // Delete the distribution list permanently
                client.DeleteDistributionList(targetList, true);
                Console.WriteLine($"Distribution list \"{targetListName}\" has been deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
