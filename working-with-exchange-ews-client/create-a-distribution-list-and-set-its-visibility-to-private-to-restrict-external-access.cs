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
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (string.IsNullOrWhiteSpace(username) || username == "username" ||
                string.IsNullOrWhiteSpace(password) || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define a private distribution list.
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Private Distribution List"
                };

                // Add members to the list.
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("member1@example.com"));
                members.Add(new MailAddress("member2@example.com"));

                // Create the distribution list on the server.
                string distributionListId = client.CreateDistributionList(distributionList, members);
                Console.WriteLine($"Distribution List created with Id: {distributionListId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
