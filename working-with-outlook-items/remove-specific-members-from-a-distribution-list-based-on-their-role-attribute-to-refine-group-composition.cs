using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials check – skip execution in CI environments
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                // Identify the distribution list to modify (replace with real Id)
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.Id = "distlist-id";

                // Fetch all current members of the distribution list
                MailAddressCollection allMembers = client.FetchDistributionList(distributionList);

                // Prepare a collection of members to delete based on role information
                MailAddressCollection membersToDelete = new MailAddressCollection();
                foreach (MailAddress member in allMembers)
                {
                    // Example criterion: remove members whose DisplayName contains "Contractor"
                    if (!string.IsNullOrEmpty(member.DisplayName) && member.DisplayName.Contains("Contractor"))
                    {
                        membersToDelete.Add(member);
                    }
                }

                // Delete the filtered members from the distribution list
                if (membersToDelete.Count > 0)
                {
                    client.DeleteFromDistributionList(distributionList, membersToDelete);
                    Console.WriteLine($"Deleted {membersToDelete.Count} member(s) from the distribution list.");
                }
                else
                {
                    Console.WriteLine("No members matched the removal criteria.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
