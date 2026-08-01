using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace DistributionListSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service connection parameters (replace with real values)
                string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and use the EWS client
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Define a new distribution list
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Bulk Email List";

                    // Initial members to add
                    MailAddressCollection initialMembers = new MailAddressCollection();
                    initialMembers.Add(new MailAddress("alice@example.com"));
                    initialMembers.Add(new MailAddress("bob@example.com"));

                    // Create the distribution list on the server
                    string distributionListId = ewsClient.CreateDistributionList(distributionList, initialMembers);
                    Console.WriteLine($"Distribution List created with Id: {distributionListId}");

                    // Add additional members (optional)
                    MailAddressCollection extraMembers = new MailAddressCollection();
                    extraMembers.Add(new MailAddress("carol@example.com"));
                    ewsClient.AddToDistributionList(distributionList, extraMembers);
                    Console.WriteLine("Additional members added to the distribution list.");

                    // Fetch and display all members of the distribution list
                    MailAddressCollection allMembers = ewsClient.FetchDistributionList(distributionList);
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
                // Graceful exit on failure
                return;
            }
        }
    }
}
