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
            // Placeholder credentials – replace with real values for actual execution
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string imapHost = "imap.example.com";
            string username = "user@example.com";

            // Guard: skip network operations when placeholders are used
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || imapHost.Contains("example") ||
                username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping network call.");
                return;
            }

            // Obtain a token provider for Google (Gmail) OAuth
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

            // Create and configure the IMAP client using the token provider
            using (ImapClient client = new ImapClient(imapHost, username, tokenProvider))
            {
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    // Validate credentials (this will perform the OAuth flow internally)
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid
                        ? "Authentication succeeded."
                        : "Authentication failed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
