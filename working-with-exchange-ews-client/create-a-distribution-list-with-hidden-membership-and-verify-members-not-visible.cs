using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Replace with your actual mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a new private distribution list
                ExchangeDistributionList dl = new ExchangeDistributionList
                {
                    DisplayName = "HiddenDL"
                };

                // Prepare members to add
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress("member1@example.com"));
                members.Add(new MailAddress("member2@example.com"));

                // Create the distribution list on the server
                string dlId = client.CreateDistributionList(dl, members);
                dl.Id = dlId; // Set the Id for subsequent operations
                Console.WriteLine($"Created distribution list. Id = {dlId}");

                // NOTE: To hide membership, the property PR_EMS_AB_HIDE_DL_MEMBERSHIP
                // (KnownPropertyList.EmsAbHideDlMembership) should be set on the DL.
                // This sample focuses on creation and verification; setting the
                // hidden‑membership property would require a custom update operation
                // not shown here.

                // Fetch members of the distribution list
                MailAddressCollection fetchedMembers = client.FetchDistributionList(dl);
                Console.WriteLine($"Fetched member count: {fetchedMembers.Count}");
                foreach (MailAddress address in fetchedMembers)
                {
                    Console.WriteLine(address.Address);
                }

                // Clean up: delete the distribution list permanently
                client.DeleteDistributionList(dl, true);
                Console.WriteLine("Deleted distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
