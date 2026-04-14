using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // List all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                if (distributionLists == null || distributionLists.Length == 0)
                {
                    Console.WriteLine("No distribution lists found.");
                    return;
                }

                // For demonstration, pick the first distribution list
                ExchangeDistributionList targetList = distributionLists[0];
                Console.WriteLine($"Target Distribution List: {targetList.DisplayName}");

                // Fetch current members of the distribution list
                MailAddressCollection currentMembers = client.FetchDistributionList(targetList);

                if (currentMembers == null || currentMembers.Count == 0)
                {
                    Console.WriteLine("The distribution list has no members to remove.");
                    return;
                }

                // Delete all members in a single batch operation
                client.DeleteFromDistributionList(targetList, currentMembers);
                Console.WriteLine("All members have been removed from the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
