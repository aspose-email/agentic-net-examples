using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailOAuthExample
{
    // Helper class to obtain OAuth token using Aspose.Email TokenProvider for Google
    public static class GoogleOAuthHelper
    {
        public static OAuthToken GetAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            // Create a Google token provider with the supplied credentials
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
            // Retrieve the OAuth token
            OAuthToken token = tokenProvider.GetAccessToken();
            return token;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values for a live run
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid external network calls during CI
                if (clientId == "clientId" || clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken" || defaultEmail == "user@example.com")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping live Gmail operations.");
                    return;
                }

                // Obtain OAuth token using the helper
                OAuthToken oauthToken = GoogleOAuthHelper.GetAccessToken(clientId, clientSecret, refreshToken);
                if (oauthToken == null || string.IsNullOrEmpty(oauthToken.Token))
                {
                    Console.Error.WriteLine("Failed to acquire OAuth token.");
                    return;
                }

                // Create Gmail client with the access token
                using (IGmailClient gmailClient = GmailClient.GetInstance(oauthToken.Token, defaultEmail))
                {
                    try
                    {
                        // List messages in the user's mailbox
                        List<GmailMessageInfo> messages = gmailClient.ListMessages();
                        Console.WriteLine($"Total messages retrieved: {messages.Count}");
                        foreach (GmailMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine($"Message Id: {messageInfo.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                        // No rethrow – graceful exit
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                // Graceful termination
            }
        }
    }
}
