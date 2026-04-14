using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder data.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve all private distribution lists.
                    ExchangeDistributionList[] allLists = client.ListDistributionLists();

                    // Find the distribution list to modify (example: first list).
                    if (allLists == null || allLists.Length == 0)
                    {
                        Console.Error.WriteLine("No distribution lists found.");
                        return;
                    }

                    ExchangeDistributionList targetList = allLists[0];

                    // Fetch current members of the distribution list.
                    MailAddressCollection currentMembers = client.FetchDistributionList(targetList);
                    if (currentMembers == null || currentMembers.Count == 0)
                    {
                        Console.WriteLine("Distribution list has no members.");
                        return;
                    }

                    // Prepare a collection of members to remove based on role attribute.
                    MailAddressCollection membersToRemove = new MailAddressCollection();

                    foreach (MailAddress member in currentMembers)
                    {
                        // Example role check: remove members whose display name contains "Manager".
                        // Adjust the condition as needed for actual role attributes.
                        if (!string.IsNullOrEmpty(member.DisplayName) && member.DisplayName.IndexOf("Manager", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            membersToRemove.Add(member);
                        }
                    }

                    if (membersToRemove.Count == 0)
                    {
                        Console.WriteLine("No members matched the removal criteria.");
                        return;
                    }

                    // Delete the selected members from the distribution list.
                    client.DeleteFromDistributionList(targetList, membersToRemove);
                    Console.WriteLine($"Removed {membersToRemove.Count} member(s) from distribution list \"{targetList.DisplayName}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
