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
            // Replace the placeholders with actual server URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip external calls when placeholders are detected.
            bool placeholders = serviceUrl.Contains("example.com") ||
                                username.Contains("example.com") ||
                                password == "password";

            if (placeholders)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create and dispose the EWS client safely.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Identify the existing distribution list by its Id.
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    Id = "YOUR-DISTRIBUTION-LIST-ID"
                    // ChangeKey can be set if required by the server.
                };

                // Prepare the members to add.
                MailAddressCollection membersToAdd = new MailAddressCollection
                {
                    new MailAddress("john.doe@example.com", "John Doe"),
                    new MailAddress("jane.smith@example.com", "Jane Smith")
                };

                // Append the members to the distribution list.
                client.AddToDistributionList(distributionList, membersToAdd);

                Console.WriteLine("Members added to the distribution list successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
