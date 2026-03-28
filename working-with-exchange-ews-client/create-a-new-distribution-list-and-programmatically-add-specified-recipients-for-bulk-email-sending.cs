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
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare the initial members collection
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));

                // Create the distribution list on the server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Distribution List created with Id: " + distributionListId);

                // Add additional members to the existing list
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("carol@example.com"));
                additionalMembers.Add(new MailAddress("dave@example.com"));

                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Additional members added to the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
