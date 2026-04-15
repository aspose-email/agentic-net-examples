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
            // EWS service connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    // ---------- Create a distribution list ----------
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Original Distribution List";

                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("member1@example.com"));
                    members.Add(new MailAddress("member2@example.com"));

                    // Create the DL on the server
                    string dlId = client.CreateDistributionList(distributionList, members);
                    distributionList.Id = dlId;

                    // ---------- Update the display name ----------
                    distributionList.DisplayName = "Updated Distribution List";

                    // Delete the old DL (move to Deleted Items)
                    client.DeleteDistributionList(distributionList, false);

                    // Re‑create the DL with the new display name
                    string newDlId = client.CreateDistributionList(distributionList, members);
                    distributionList.Id = newDlId;

                    // ---------- Verify propagation ----------
                    var allLists = client.ListDistributionLists();
                    Console.WriteLine("Distribution Lists in the mailbox:");
                    foreach (var dl in allLists)
                    {
                        Console.WriteLine($"- DisplayName: {dl.DisplayName}, Id: {dl.Id}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
