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
            // EWS connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Distribution list to update and the new member's SMTP address
            string distributionListDisplayName = "Team DL";
            string newMemberSmtp = "newmember@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || newMemberSmtp.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all private distribution lists
                ExchangeDistributionList[] lists = client.ListDistributionLists();

                // Find the target distribution list by display name
                ExchangeDistributionList targetList = null;
                foreach (ExchangeDistributionList dl in lists)
                {
                    if (dl.DisplayName == distributionListDisplayName)
                    {
                        targetList = dl;
                        break;
                    }
                }

                if (targetList == null)
                {
                    Console.Error.WriteLine("Distribution list not found.");
                    return;
                }

                // Prepare the collection of members to add (only SMTP address required)
                MailAddressCollection members = new MailAddressCollection();
                members.Add(new MailAddress(newMemberSmtp));

                // Add the member to the distribution list
                client.AddToDistributionList(targetList, members);
                Console.WriteLine("Member added successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
