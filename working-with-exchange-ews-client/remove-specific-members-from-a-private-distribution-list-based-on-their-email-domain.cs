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
            // Connection parameters (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Target distribution list identifier and domain to remove
            string targetDistributionListId = "distribution-list-id";
            string domainToRemove = "unwanted.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a distribution list object with the known Id
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    Id = targetDistributionListId
                };

                // Fetch current members of the private distribution list
                MailAddressCollection currentMembers = client.FetchDistributionList(distributionList);

                // Collect members whose email address ends with the unwanted domain
                MailAddressCollection membersToRemove = new MailAddressCollection();
                foreach (MailAddress address in currentMembers)
                {
                    if (address.Address != null &&
                        address.Address.EndsWith("@" + domainToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        membersToRemove.Add(address);
                    }
                }

                // Remove the selected members if any were found
                if (membersToRemove.Count > 0)
                {
                    client.DeleteFromDistributionList(distributionList, membersToRemove);
                    Console.WriteLine($"Removed {membersToRemove.Count} member(s) from the distribution list.");
                }
                else
                {
                    Console.WriteLine("No members matched the specified domain.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
