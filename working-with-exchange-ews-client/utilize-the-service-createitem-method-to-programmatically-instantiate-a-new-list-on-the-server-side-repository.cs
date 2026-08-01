using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client (replace with actual values)
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string domain = "example.com";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || domain.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, domain))
                {
                    // Create a new distribution list object
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Sample Distribution List";

                    // Prepare members for the distribution list
                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("member1@example.com"));
                    members.Add(new MailAddress("member2@example.com"));

                    // Create the distribution list on the server
                    string listId = client.CreateDistributionList(distributionList, members);
                    Console.WriteLine($"Distribution List created with Id: {listId}");

                    // Optionally fetch and display the created list members
                    MailAddressCollection fetchedMembers = client.FetchDistributionList(distributionList);
                    Console.WriteLine("Members of the created list:");
                    foreach (MailAddress address in fetchedMembers)
                    {
                        Console.WriteLine($"- {address.Address}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
