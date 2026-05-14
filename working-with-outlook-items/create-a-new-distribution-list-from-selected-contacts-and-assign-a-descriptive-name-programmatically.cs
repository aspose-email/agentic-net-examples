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
            // Service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid external calls during CI
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder service URL detected. Skipping execution.");
                return;
            }

            // Create and authenticate the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define a new distribution list with a descriptive name
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Project Team"
                };

                // Prepare initial members for the distribution list
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("alice@example.com", "Alice"));
                members.Add(new MailAddress("bob@example.com", "Bob"));
                members.Add(new MailAddress("carol@example.com", "Carol"));

                // Create the distribution list on the Exchange server
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine($"Distribution List created with Id: {distributionListId}");

                // Optionally add more members later
                MailAddressCollection additionalMembers = new MailAddressCollection();
                additionalMembers.Add(new MailAddress("dave@example.com", "Dave"));
                client.AddToDistributionList(distributionList, additionalMembers);
                Console.WriteLine("Additional members added to the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
