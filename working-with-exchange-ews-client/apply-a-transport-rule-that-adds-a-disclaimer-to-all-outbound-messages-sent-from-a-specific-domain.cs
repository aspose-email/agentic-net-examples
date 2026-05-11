using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – in real scenario replace with actual values.
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials.
            if (username.Contains("username") || password.Contains("password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping rule creation.");
                return;
            }

            // Create EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Build the inbox rule.
            InboxRule rule = new InboxRule
            {
                DisplayName = "Add disclaimer for contoso.com",
                IsEnabled = true,
                Priority = 1,
                // Conditions and actions are simplified due to API limitations.
                // In a full implementation, set rule.Conditions.FromAddressContains = "contoso.com"
                // and configure an appropriate action to add a disclaimer if supported.
                // Example: you could set a server reply with a template that contains the disclaimer.
            };

            // Create the rule on the server.
            try
            {
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create inbox rule: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed.
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
