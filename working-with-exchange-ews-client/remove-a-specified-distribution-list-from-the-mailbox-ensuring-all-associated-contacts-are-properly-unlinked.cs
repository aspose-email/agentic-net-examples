using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create network credentials (avoid naming conflict with any existing 'credential' variable)
                NetworkCredential networkCredential = new NetworkCredential(username, password);

                // Initialize EWS client inside a using block for proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, networkCredential))
                {
                    // List all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    // Find the distribution list to remove (replace with actual display name)
                    ExchangeDistributionList targetList = null;
                    foreach (ExchangeDistributionList dl in distributionLists)
                    {
                        if (dl.DisplayName == "My Distribution List")
                        {
                            targetList = dl;
                            break;
                        }
                    }

                    if (targetList == null)
                    {
                        Console.WriteLine("Distribution list not found.");
                        return;
                    }

                    // Fetch members of the distribution list
                    MailAddressCollection members = client.FetchDistributionList(targetList);

                    // Remove all members from the distribution list (if any)
                    if (members != null && members.Count > 0)
                    {
                        client.DeleteFromDistributionList(targetList, members);
                    }

                    // Delete the distribution list permanently
                    client.DeleteDistributionList(targetList, true);

                    Console.WriteLine("Distribution list removed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}