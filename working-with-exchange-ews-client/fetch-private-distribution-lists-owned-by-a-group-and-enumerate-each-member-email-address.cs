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
            // EWS service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // List all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    foreach (ExchangeDistributionList dl in distributionLists)
                    {
                        Console.WriteLine($"Distribution List: {dl.DisplayName}");

                        // Fetch members of the private distribution list
                        MailAddressCollection members = client.FetchDistributionList(dl);

                        foreach (MailAddress member in members)
                        {
                            Console.WriteLine($"  Member Email: {member.Address}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
