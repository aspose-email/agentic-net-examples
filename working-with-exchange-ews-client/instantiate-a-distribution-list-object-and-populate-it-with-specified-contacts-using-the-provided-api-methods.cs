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
            // Placeholder Exchange service URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange service URL detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            NetworkCredential credential = new NetworkCredential(username, password);
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Create a distribution list object.
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare members for the distribution list.
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com"));
                members.Add(new MailAddress("bob@example.com"));
                members.Add(new MailAddress("carol@example.com"));

                // Create the distribution list on the server.
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine("Distribution List created with Id: " + distributionListId);

                // Assign the returned Id to the distribution list object for further operations.
                distributionList.Id = distributionListId;

                // Fetch members to verify creation.
                MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                Console.WriteLine("Fetched members:");
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine("- " + address.DisplayName + " <" + address.Address + ">");
                }

                // Example of adding additional members.
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("dave@example.com"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Added additional member to the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
