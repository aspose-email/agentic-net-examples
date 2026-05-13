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
            // Placeholder connection settings – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string distributionListId = "placeholder-id";

            // Skip execution when placeholders are detected.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || distributionListId == "placeholder-id")
            {
                Console.Error.WriteLine("Placeholder credentials or URLs detected. Skipping execution.");
                return;
            }

            // Create the Exchange client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Prepare the distribution list reference.
                ExchangeDistributionList distList = new ExchangeDistributionList
                {
                    Id = distributionListId
                };

                // Fetch current members.
                MailAddressCollection members = client.FetchDistributionList(distList);
                if (members == null || members.Count == 0)
                {
                    Console.WriteLine("No members found in the distribution list.");
                    return;
                }

                // Identify duplicates based on case‑insensitive email address.
                var uniqueMembers = new System.Collections.Generic.Dictionary<string, MailAddress>(StringComparer.OrdinalIgnoreCase);
                MailAddressCollection duplicates = new MailAddressCollection();

                foreach (MailAddress address in members)
                {
                    string emailKey = address.Address?.Trim().ToLowerInvariant() ?? string.Empty;
                    if (uniqueMembers.ContainsKey(emailKey))
                    {
                        // Duplicate found – schedule for removal.
                        duplicates.Add(address);
                    }
                    else
                    {
                        uniqueMembers[emailKey] = address;
                    }
                }

                if (duplicates.Count == 0)
                {
                    Console.WriteLine("No duplicate members to remove.");
                    return;
                }

                // Remove duplicate members from the distribution list.
                client.DeleteFromDistributionList(distList, duplicates);
                Console.WriteLine($"Removed {duplicates.Count} duplicate member(s) from the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
