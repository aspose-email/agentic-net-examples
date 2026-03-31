using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard: skip network calls when placeholders are used.
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail API calls.");
                return;
            }

            // Obtain a token provider for Google and retrieve an access token.
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
            OAuthToken oauthToken = tokenProvider.GetAccessToken();

            // Create the Gmail client using the obtained access token.
            using (IGmailClient gmailClient = GmailClient.GetInstance(oauthToken.Token, defaultEmail))
            {
                // List messages in the mailbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                Console.WriteLine($"Total messages: {messages.Count}");
                foreach (GmailMessageInfo info in messages)
                {
                    // GmailMessageInfo does not expose Subject; use Id for demonstration.
                    Console.WriteLine($"Message Id: {info.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
