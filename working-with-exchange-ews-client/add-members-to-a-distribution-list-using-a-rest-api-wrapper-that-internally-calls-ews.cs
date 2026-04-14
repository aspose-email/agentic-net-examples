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
            // Exchange server connection settings
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a new distribution list (or fetch an existing one)
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Sample Distribution List"
                };

                // Initial members for the distribution list
                MailAddressCollection initialMembers = new MailAddressCollection
                {
                    new MailAddress("member1@example.com"),
                    new MailAddress("member2@example.com")
                };

                // Create the distribution list with initial members
                string dlId = client.CreateDistributionList(distributionList, initialMembers);
                distributionList.Id = dlId; // Store the returned Id for further operations

                // New members to add to the existing distribution list
                MailAddressCollection newMembers = new MailAddressCollection
                {
                    new MailAddress("newmember1@example.com"),
                    new MailAddress("newmember2@example.com")
                };

                // Add new members to the distribution list
                client.AddToDistributionList(distributionList, newMembers);

                Console.WriteLine("Members added successfully to the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
