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
            // Initialize EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", new NetworkCredential("user@example.com", "password")))
            {
                // Prepare initial members for the distribution list
                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("alice@example.com"));
                initialMembers.Add(new MailAddress("bob@example.com"));

                // Define a new distribution list
                ExchangeDistributionList dl = new ExchangeDistributionList
                {
                    DisplayName = "Sample Distribution List"
                };

                // Create the distribution list on the server
                string dlId = client.CreateDistributionList(dl, initialMembers);
                Console.WriteLine($"Created Distribution List with Id: {dlId}");

                // Prepare additional members to add
                MailAddressCollection newMembers = new MailAddressCollection();
                newMembers.Add(new MailAddress("carol@example.com"));
                newMembers.Add(new MailAddress("dave@example.com"));

                // Update the distribution list by adding new members
                dl.Id = dlId; // Set the Id of the existing list
                client.AddToDistributionList(dl, newMembers);
                Console.WriteLine("Added new members to the distribution list.");

                // Optionally fetch and display the updated members
                MailAddressCollection allMembers = client.FetchDistributionList(dl);
                Console.WriteLine("Current members of the distribution list:");
                foreach (MailAddress address in allMembers)
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
