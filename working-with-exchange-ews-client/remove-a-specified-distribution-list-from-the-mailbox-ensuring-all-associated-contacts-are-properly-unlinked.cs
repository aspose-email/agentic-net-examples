using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

// Author: Aspose.Email example - remove a distribution list and unlink its members
class Program
{
    static void Main()
    {
        // EWS connection parameters (replace with real values)
        string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";
        string domain = "example.com";

        // Distribution List identifiers (replace with actual Id and ChangeKey)
        string distributionListId = "DL_ID";
        string distributionListChangeKey = "CHANGE_KEY";


        // Skip external calls when placeholder credentials are used
        if (ewsUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || domain.Contains("example.com"))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Create EWS client (IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password, domain))
            {
                // Build a distribution list object with known Id and ChangeKey
                ExchangeDistributionList distList = new ExchangeDistributionList();
                distList.Id = distributionListId;
                distList.ChangeKey = distributionListChangeKey;

                // Retrieve current members of the distribution list
                MailAddressCollection members = client.FetchDistributionList(distList);

                // If there are members, remove them to unlink contacts
                if (members != null && members.Count > 0)
                {
                    client.DeleteFromDistributionList(distList, members);
                }

                // Delete the distribution list permanently
                client.DeleteDistributionList(distList, true);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
