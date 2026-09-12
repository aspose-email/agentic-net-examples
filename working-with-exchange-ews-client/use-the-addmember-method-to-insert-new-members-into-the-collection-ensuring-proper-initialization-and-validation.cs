using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailDistributionListExample
{
    // Extension method to simplify adding members with validation
    public static class MailAddressCollectionExtensions
    {
        public static void AddMember(this MailAddressCollection collection, string email, string displayName = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be null or empty.", nameof(email));

            // Basic validation – ensure the email contains '@'
            if (!email.Contains("@"))
                throw new ArgumentException("Invalid email address format.", nameof(email));

            collection.Add(new MailAddress(email, displayName));
        }
    }

    class Program
    {
        static void Main()
        {
            // ----- Configuration (replace with real values or set via environment variables) -----
            string serviceUrl = Environment.GetEnvironmentVariable("EWS_SERVICE_URL") ?? "https://your-ews-server/EWS/Exchange.asmx";
            string username   = Environment.GetEnvironmentVariable("EWS_USERNAME")   ?? "user@example.com";
            string password   = Environment.GetEnvironmentVariable("EWS_PASSWORD")   ?? "your_password";

            // Guard against placeholder credentials
            if (serviceUrl.Contains("your") || username.Contains("your") || password.Contains("your"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // ----- Create a distribution list and add members -----
            ExchangeDistributionList distributionList = new ExchangeDistributionList
            {
                DisplayName = "Sample Distribution List"
            };

            MailAddressCollection members = new MailAddressCollection();
            try
            {
                // Use the AddMember extension to insert members with validation
                members.AddMember("john.doe@example.com", "John Doe");
                members.AddMember("jane.smith@example.com", "Jane Smith");
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Member validation error: {ex.Message}");
                return;
            }

            // ----- Connect to EWS and create the distribution list -----
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Create the distribution list on the server; returns the new list Id
                    string listId = client.CreateDistributionList(distributionList, members);
                    Console.WriteLine($"Distribution list created with Id: {listId}");

                    // Example of adding additional members later using AddToDistributionList
                    MailAddressCollection additionalMembers = new MailAddressCollection();
                    additionalMembers.AddMember("alice.wonderland@example.com", "Alice Wonderland");

                    client.AddToDistributionList(distributionList, additionalMembers);
                    Console.WriteLine("Additional member added successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                // Do not rethrow; exit gracefully
            }
        }
    }
}
