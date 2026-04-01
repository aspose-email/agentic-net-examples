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
            // Placeholder connection details – replace with real values when available.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client inside a using block to ensure proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Initialize a new distribution list.
                    ExchangeDistributionList distributionList = new ExchangeDistributionList
                    {
                        DisplayName = "Sample Distribution List"
                    };

                    // Prepare the collection of members to add.
                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("alice@example.com"));
                    members.Add(new MailAddress("bob@example.com"));

                    // Add members to the distribution list on the server.
                    client.AddToDistributionList(distributionList, members);

                    Console.WriteLine("Members added to the distribution list successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while adding members: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
