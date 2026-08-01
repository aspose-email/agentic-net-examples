using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Google;

namespace GmailProvisionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace the placeholder values with actual credentials from your Google Cloud Console.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "your.email@example.com";

                // Guard against placeholder literals.
                if (clientId.StartsWith("YOUR_") ||
                    clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_") ||
                    defaultEmail.StartsWith("your.email@"))
                {
                    Console.Error.WriteLine("Please replace placeholder values with actual credentials.");
                    return;
                }

                // Obtain an instance of the Gmail client.
                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

                // Retrieve the list of messages from the inbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                Console.WriteLine($"Retrieved {messages.Count} messages from the inbox.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
