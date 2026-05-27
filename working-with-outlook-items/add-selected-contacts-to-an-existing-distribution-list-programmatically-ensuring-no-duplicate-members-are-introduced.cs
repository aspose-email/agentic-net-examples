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
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Email address of the existing distribution list
                string dlEmail = "team@example.com";

                // Prepare a minimal ExchangeDistributionList instance with the identifier
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.Id = dlEmail;

                // Fetch current members of the distribution list
                MailAddressCollection existingMembers = client.FetchDistributionList(distributionList);

                // Define new contacts to add
                MailAddressCollection newMembers = new MailAddressCollection();
                newMembers.Add(new MailAddress("alice@example.com"));
                newMembers.Add(new MailAddress("bob@example.com"));

                // Determine which new members are not already present
                MailAddressCollection membersToAdd = new MailAddressCollection();
                foreach (MailAddress candidate in newMembers)
                {
                    bool alreadyExists = false;
                    foreach (MailAddress existing in existingMembers)
                    {
                        if (string.Equals(existing.Address, candidate.Address, StringComparison.OrdinalIgnoreCase))
                        {
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (!alreadyExists)
                    {
                        membersToAdd.Add(candidate);
                    }
                }

                // Add only the non‑duplicate members
                if (membersToAdd.Count > 0)
                {
                    client.AddToDistributionList(distributionList, membersToAdd);
                    Console.WriteLine($"Added {membersToAdd.Count} new member(s) to the distribution list.");
                }
                else
                {
                    Console.WriteLine("No new members to add; all candidates are already members.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
