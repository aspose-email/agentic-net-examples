using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string exchangeUrl = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (exchangeUrl.Contains("your.") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Exchange credentials are placeholders. Skipping execution.");
                return;
            }

            // Create and connect the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(exchangeUrl, username, password))
            {
                // Define a new distribution list with a descriptive name.
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Project Team";

                // Prepare the list of members to add.
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));
                members.Add(new MailAddress("carol@example.com"));

                // Create the distribution list on the Exchange server.
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Distribution List created with Id: " + distributionListId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
