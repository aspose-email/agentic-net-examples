using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client inside a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // List private distribution lists
                ExchangeDistributionList[] dlists = client.ListDistributionLists();

                if (dlists == null || dlists.Length == 0)
                {
                    Console.WriteLine("No distribution lists found.");
                    return;
                }

                // Take the first distribution list for demonstration
                ExchangeDistributionList dl = dlists[0];
                Console.WriteLine($"Distribution List: {dl.DisplayName}");

                // Fetch the distribution list item as a MAPI message
                MapiMessage dlMessage = client.FetchItem(dl.Id) as MapiMessage;
                if (dlMessage == null)
                {
                    Console.WriteLine("Failed to fetch distribution list item.");
                    return;
                }

                // Retrieve the distribution list name (custom attribute)
                object nameObj = dlMessage.GetProperty(KnownPropertyList.DistributionListName);
                string dlName = nameObj != null ? nameObj.ToString() : "<unknown>";
                Console.WriteLine($"DL Name Property: {dlName}");

                // Retrieve the total member count (extended property)
                object countObj = dlMessage.GetProperty(KnownPropertyList.AddressBookDistributionListMemberCount);
                int memberCount = countObj != null ? Convert.ToInt32(countObj) : 0;
                Console.WriteLine($"Member Count Property: {memberCount}");

                // Retrieve members using the dedicated API
                MailAddressCollection members = client.FetchDistributionList(dl);
                Console.WriteLine($"Actual Members Count (via API): {members.Count}");
                foreach (var address in members)
                {
                    Console.WriteLine($" - {address.DisplayName} <{address.Address}>");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
