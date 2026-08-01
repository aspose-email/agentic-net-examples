using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace DistributionListSample
{
    // Author: Aspose.Email example
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service endpoint and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Prepare a new distribution list
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Sample Distribution List";

                    // Initial members
                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("alice@example.com"));
                    members.Add(new MailAddress("bob@example.com"));

                    // Create the distribution list on the server
                    string listId = ewsClient.CreateDistributionList(distributionList, members);
                    Console.WriteLine($"Distribution List created with Id: {listId}");

                    // Fetch and display current members
                    MailAddressCollection fetchedMembers = ewsClient.FetchDistributionList(distributionList);
                    Console.WriteLine("Current members:");
                    foreach (MailAddress address in fetchedMembers)
                    {
                        Console.WriteLine($"- {address.Address}");
                    }

                    // Add an additional member
                    MailAddressCollection additionalMembers = new MailAddressCollection();
                    additionalMembers.Add(new MailAddress("carol@example.com"));
                    ewsClient.AddToDistributionList(distributionList, additionalMembers);
                    Console.WriteLine("Added a new member.");

                    // Fetch and display updated members
                    MailAddressCollection updatedMembers = ewsClient.FetchDistributionList(distributionList);
                    Console.WriteLine("Updated members:");
                    foreach (MailAddress address in updatedMembers)
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
}
