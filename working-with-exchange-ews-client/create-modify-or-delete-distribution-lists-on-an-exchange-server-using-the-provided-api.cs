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
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange server URL detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Create a new distribution list
                    ExchangeDistributionList dl = new ExchangeDistributionList
                    {
                        DisplayName = "Sample Distribution List"
                    };

                    // Initial members
                    MailAddressCollection initialMembers = new MailAddressCollection();
                    initialMembers.Add(new MailAddress("member1@example.com"));
                    initialMembers.Add(new MailAddress("member2@example.com"));

                    // Create the distribution list on the server
                    string dlId = client.CreateDistributionList(dl, initialMembers);
                    Console.WriteLine($"Created Distribution List with Id: {dlId}");

                    // Fetch current members
                    MailAddressCollection currentMembers = client.FetchDistributionList(dl);
                    Console.WriteLine("Current members:");
                    foreach (MailAddress addr in currentMembers)
                    {
                        Console.WriteLine($"- {addr.Address}");
                    }

                    // Add additional members
                    MailAddressCollection membersToAdd = new MailAddressCollection();
                    membersToAdd.Add(new MailAddress("member3@example.com"));
                    client.AddToDistributionList(dl, membersToAdd);
                    Console.WriteLine("Added member3@example.com");

                    // Delete a member
                    MailAddressCollection membersToRemove = new MailAddressCollection();
                    membersToRemove.Add(new MailAddress("member1@example.com"));
                    client.DeleteFromDistributionList(dl, membersToRemove);
                    Console.WriteLine("Removed member1@example.com");

                    // Fetch updated members
                    MailAddressCollection updatedMembers = client.FetchDistributionList(dl);
                    Console.WriteLine("Updated members:");
                    foreach (MailAddress addr in updatedMembers)
                    {
                        Console.WriteLine($"- {addr.Address}");
                    }

                    // Delete the distribution list (move to Deleted Items)
                    client.DeleteDistributionList(dl, false);
                    Console.WriteLine("Distribution List moved to Deleted Items.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
