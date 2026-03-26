using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    // Define a new distribution list
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Sample Distribution List";

                    // Initial members for the distribution list
                    MailAddressCollection initialMembers = new MailAddressCollection();
                    initialMembers.Add(new MailAddress("alice@example.com"));
                    initialMembers.Add(new MailAddress("bob@example.com"));

                    // Create the distribution list on the server
                    string distributionListId = client.CreateDistributionList(distributionList, initialMembers);
                    Console.WriteLine("Distribution List created with Id: " + distributionListId);

                    // Additional members to add later
                    MailAddressCollection additionalMembers = new MailAddressCollection();
                    additionalMembers.Add(new MailAddress("carol@example.com"));
                    additionalMembers.Add(new MailAddress("dave@example.com"));

                    // Append additional members to the existing distribution list
                    client.AddToDistributionList(distributionList, additionalMembers);
                    Console.WriteLine("Additional members added to the distribution list.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}