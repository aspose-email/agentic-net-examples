using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://your-ews-server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder values to avoid external calls during CI.
            if (serviceUrl.Contains("your") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            NetworkCredential credential = new NetworkCredential(username, password);

            // Create and connect the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // List all private distribution lists.
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                // Identify the distribution list to remove (by display name).
                string targetDisplayName = "Sample Distribution List";
                ExchangeDistributionList targetList = null;

                foreach (ExchangeDistributionList dl in distributionLists)
                {
                    if (dl.DisplayName != null && dl.DisplayName.Equals(targetDisplayName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetList = dl;
                        break;
                    }
                }

                if (targetList == null)
                {
                    Console.Error.WriteLine($"Distribution list \"{targetDisplayName}\" not found.");
                    return;
                }

                // Fetch members of the distribution list.
                MailAddressCollection members = client.FetchDistributionList(targetList);

                // Remove members from the distribution list to unlink contacts.
                if (members != null && members.Count > 0)
                {
                    client.DeleteFromDistributionList(targetList, members);
                }

                // Delete the distribution list permanently.
                bool deletePermanently = true;
                client.DeleteDistributionList(targetList, deletePermanently);

                Console.WriteLine($"Distribution list \"{targetDisplayName}\" has been removed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
