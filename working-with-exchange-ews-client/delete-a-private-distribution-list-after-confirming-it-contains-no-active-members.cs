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
            // Define EWS connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // List all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    // Specify the display name of the distribution list to delete
                    string targetDisplayName = "MyPrivateList";


                    // Skip external calls when placeholder credentials are used
                    if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    foreach (ExchangeDistributionList list in distributionLists)
                    {
                        if (list.DisplayName != null && list.DisplayName.Equals(targetDisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Fetch members of the distribution list
                            MailAddressCollection members = client.FetchDistributionList(list);

                            if (members == null || members.Count == 0)
                            {
                                // No active members – delete the distribution list permanently
                                client.DeleteDistributionList(list, true);
                                Console.WriteLine($"Distribution list \"{targetDisplayName}\" deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Distribution list \"{targetDisplayName}\" has {members.Count} member(s). Deletion aborted.");
                            }

                            // Assuming only one list with the given name; exit loop
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
