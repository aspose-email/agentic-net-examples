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
            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Define the members of the distribution list
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("user1@example.com"));
                members.Add(new MailAddress("user2@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Created Distribution List Id: " + distributionListId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
