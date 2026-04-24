using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Yahoo IMAP server details
            string host = "imap.mail.yahoo.com";
            int port = 993;
            string username = "your_username";
            string oauthToken = "your_oauth_token";

            // Guard against placeholder credentials
            if (username.StartsWith("your_") || oauthToken.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping validation.");
                return;
            }

            // Initialize the IMAP client with OAuth2 (two‑factor authentication)
            using (ImapClient client = new ImapClient(host, port, username, oauthToken, true, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Validate the credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
