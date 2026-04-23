using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string userEmail = "your-email@gmail.com";

            // Guard against placeholder values to avoid unwanted network calls.
            if (clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") ||
                userEmail.StartsWith("your-"))
            {
                Console.Error.WriteLine("Please provide valid Google OAuth credentials before running the sample.");
                return;
            }

            // Obtain a token provider for Google OAuth.
            using (TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken))
            {
                // Retrieve the OAuth token.
                OAuthToken oauthToken = tokenProvider.GetAccessToken();

                // Create an IMAP client using the OAuth token.
                using (ImapClient client = new ImapClient(
                    "imap.gmail.com",
                    993,
                    userEmail,
                    oauthToken.Token,
                    true,
                    SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate the credentials.
                        bool isValid = client.ValidateCredentials();
                        Console.WriteLine(isValid
                            ? "IMAP credentials are valid."
                            : "IMAP credentials are invalid.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
