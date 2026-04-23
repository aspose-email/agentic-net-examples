using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GmailRetrySample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                // Guard against running with placeholder data.
                if (clientId.StartsWith("YOUR_") ||
                    clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_") ||
                    defaultEmail.StartsWith("user@"))
                {
                    Console.Error.WriteLine("Please provide valid Google OAuth credentials before running the sample.");
                    return;
                }

                // Obtain an OAuth token.
                TokenProvider googleProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
                OAuthToken oauthToken;
                try
                {
                    oauthToken = googleProvider.GetAccessToken();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to acquire access token: {ex.Message}");
                    return;
                }

                // Create Gmail client using the access token.
                IGmailClient gmailClient;
                try
                {
                    gmailClient = GmailClient.GetInstance(oauthToken.Token, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                using (gmailClient)
                {
                    // Prepare a simple email message.
                    MailMessage message = new MailMessage();
                    message.From = defaultEmail;
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email with Retry";
                    message.Body = "This email demonstrates token refresh retry logic.";

                    // Attempt to send the message with a single retry on token expiration.
                    const int maxAttempts = 2;
                    int attempt = 0;
                    bool sent = false;

                    while (attempt < maxAttempts && !sent)
                    {
                        attempt++;
                        try
                        {
                            string messageId = gmailClient.SendMessage(message);
                            Console.WriteLine($"Message sent successfully. Id: {messageId}");
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            // Simplified check: assume any exception may be due to token expiration.
                            Console.Error.WriteLine($"Send attempt {attempt} failed: {ex.Message}");

                            if (attempt < maxAttempts)
                            {
                                try
                                {
                                    // Refresh the access token and retry.
                                    gmailClient.RefreshToken();
                                    Console.WriteLine("Access token refreshed. Retrying send operation.");
                                }
                                catch (Exception refreshEx)
                                {
                                    Console.Error.WriteLine($"Failed to refresh token: {refreshEx.Message}");
                                    break;
                                }
                            }
                            else
                            {
                                Console.Error.WriteLine("All send attempts failed.");
                            }
                        }
                    }

                    // Dispose the message.
                    message.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
