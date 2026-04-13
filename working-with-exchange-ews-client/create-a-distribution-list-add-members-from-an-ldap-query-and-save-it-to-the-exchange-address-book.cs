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
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Prepare the distribution list object
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Sample Distribution List";

                    // Simulate LDAP query results with a few mail addresses
                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("alice@example.com", "Alice"));
                    members.Add(new MailAddress("bob@example.com", "Bob"));
                    members.Add(new MailAddress("carol@example.com", "Carol"));

                    // Create the distribution list on the Exchange server
                    string distributionListId = client.CreateDistributionList(distributionList, members);
                    Console.WriteLine("Distribution List created. Id: " + distributionListId);

                    // Optionally fetch and display the members of the created list
                    MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                    Console.WriteLine("Fetched members count: " + fetchedMembers.Count);
                    foreach (MailAddress address in fetchedMembers)
                    {
                        Console.WriteLine($"{address.DisplayName} <{address.Address}>");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("EWS client error: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
