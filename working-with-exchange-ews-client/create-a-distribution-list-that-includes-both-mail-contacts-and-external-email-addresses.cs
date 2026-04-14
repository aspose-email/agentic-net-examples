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
            // Initialize EWS client
            IEWSClient client;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Create a new distribution list object
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Sample Distribution List"
                };

                // Prepare members collection (both internal contacts and external addresses)
                MailAddressCollection members = new MailAddressCollection();
                // Internal contact (replace with a valid internal email address)
                members.Add(new MailAddress("internal.user@example.com", "Internal User"));
                // External email address
                members.Add(new MailAddress("external.user@example.org", "External User"));

                // Create the distribution list on the server
                try
                {
                    string dlId = client.CreateDistributionList(distributionList, members);
                    Console.WriteLine($"Distribution List created with Id: {dlId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
