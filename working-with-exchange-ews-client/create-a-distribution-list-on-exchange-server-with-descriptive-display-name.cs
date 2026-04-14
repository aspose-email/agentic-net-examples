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
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define the distribution list with a descriptive display name
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Project Team Distribution List";

                // Prepare the list of members
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));
                members.Add(new MailAddress("carol@example.com"));

                // Create the distribution list on the Exchange server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Distribution List created. Id: " + distributionListId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
