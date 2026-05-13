using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, username, password))
            {
                // Retrieve all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                // Collection to hold unique members
                MailAddressCollection combinedMembers = new MailAddressCollection();
                HashSet<string> seenEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Iterate through each distribution list and collect unique members
                foreach (ExchangeDistributionList list in distributionLists)
                {
                    MailAddressCollection members = client.FetchDistributionList(list);
                    foreach (MailAddress member in members)
                    {
                        if (member != null && !string.IsNullOrEmpty(member.Address) && seenEmails.Add(member.Address))
                        {
                            combinedMembers.Add(member);
                        }
                    }
                }

                // Create a new unified distribution list with the combined members
                ExchangeDistributionList unifiedList = new ExchangeDistributionList();
                unifiedList.DisplayName = "Unified Distribution List";

                string newListId = client.CreateDistributionList(unifiedList, combinedMembers);
                Console.WriteLine($"Unified distribution list created with Id: {newListId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
