using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize credentials for the Exchange server
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create the EWS client inside a using block to ensure proper disposal
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credential))
                {
                    // Identify the existing distribution list by its Id
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.Id = "distribution-list-id";

                    // Prepare the collection of new members to add
                    MailAddressCollection members = new MailAddressCollection();
                    members.Add(new MailAddress("newmember1@example.com"));
                    members.Add(new MailAddress("newmember2@example.com"));

                    // Append the new members to the distribution list
                    client.AddToDistributionList(distributionList, members);

                    Console.WriteLine("Members added to the distribution list successfully.");
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
