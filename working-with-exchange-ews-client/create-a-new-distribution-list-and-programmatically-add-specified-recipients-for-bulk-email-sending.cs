using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace DistributionListSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define EWS service URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Initialize a new distribution list
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.DisplayName = "Bulk Email List";

                    // Prepare initial members
                    MailAddressCollection initialMembers = new MailAddressCollection();
                    initialMembers.Add(new MailAddress("alice@example.com"));
                    initialMembers.Add(new MailAddress("bob@example.com"));

                    // Create the distribution list on the server
                    string distributionListId = client.CreateDistributionList(distributionList, initialMembers);
                    Console.WriteLine("Created distribution list with Id: " + distributionListId);

                    // Add additional members to the existing list
                    MailAddressCollection additionalMembers = new MailAddressCollection();
                    additionalMembers.Add(new MailAddress("carol@example.com"));
                    additionalMembers.Add(new MailAddress("dave@example.com"));

                    client.AddToDistributionList(distributionList, additionalMembers);
                    Console.WriteLine("Added additional members to the distribution list.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
