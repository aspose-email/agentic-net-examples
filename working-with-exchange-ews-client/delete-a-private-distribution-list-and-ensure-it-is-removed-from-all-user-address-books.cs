using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Name of the distribution list to delete
                    string dlDisplayName = "Sample Private Distribution List";


                    // Skip external calls when placeholder credentials are used
                    if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Retrieve all private distribution lists
                    ExchangeDistributionList[] dlLists = client.ListDistributionLists();

                    // Find the distribution list with the specified display name
                    ExchangeDistributionList targetDl = null;
                    foreach (ExchangeDistributionList dl in dlLists)
                    {
                        if (string.Equals(dl.DisplayName, dlDisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            targetDl = dl;
                            break;
                        }
                    }

                    if (targetDl == null)
                    {
                        Console.Error.WriteLine($"Distribution list \"{dlDisplayName}\" not found.");
                        return;
                    }

                    // Delete the distribution list permanently
                    client.DeleteDistributionList(targetDl, deletePermanently: true);
                    Console.WriteLine($"Distribution list \"{dlDisplayName}\" has been deleted.");
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
