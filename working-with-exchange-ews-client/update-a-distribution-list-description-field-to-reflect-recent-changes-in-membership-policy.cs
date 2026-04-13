using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Distribution list details
            string dlDisplayName = "Team Distribution List";
            string newDescription = "Updated membership policy: members must be active staff only.";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create a collection of members
            MailAddressCollection members = new MailAddressCollection();
            members.Add(new MailAddress("alice@example.com"));
            members.Add(new MailAddress("bob@example.com"));

            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client as IDisposable)
            {
                try
                {
                    // Create a new distribution list with the description embedded in the display name
                    ExchangeDistributionList dl = new ExchangeDistributionList
                    {
                        DisplayName = $"{dlDisplayName} - {newDescription}"
                    };

                    // Create the distribution list on the server
                    string dlId = client.CreateDistributionList(dl, members);
                    Console.WriteLine($"Distribution list created with Id: {dlId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
