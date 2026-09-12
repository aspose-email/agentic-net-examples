using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URI and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a new inbox rule
                InboxRule newRule = new InboxRule();
                newRule.DisplayName = "Sample rule";

                client.CreateInboxRule(newRule);
                Console.WriteLine("Inbox rule created.");

                // Retrieval of inbox rules is not demonstrated here because the required API method is not available in this version.

                // Update the previously created rule
                newRule.DisplayName = "Updated sample rule";
                client.UpdateInboxRule(newRule);
                Console.WriteLine("Inbox rule updated.");

                // Deletion of inbox rules is not demonstrated here because the required API method is not available in this version.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
