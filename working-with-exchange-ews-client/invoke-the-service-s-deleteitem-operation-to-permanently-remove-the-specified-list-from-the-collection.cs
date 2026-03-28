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
            // Server URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Retrieve distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    if (distributionLists == null || distributionLists.Length == 0)
                    {
                        Console.WriteLine("No distribution lists found.");
                        return;
                    }

                    // Select the first list for demonstration
                    ExchangeDistributionList listToDelete = distributionLists[0];
                    Console.WriteLine($"Deleting distribution list: {listToDelete.DisplayName}");

                    // Delete permanently
                    client.DeleteDistributionList(listToDelete, true);
                    Console.WriteLine("Distribution list deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
