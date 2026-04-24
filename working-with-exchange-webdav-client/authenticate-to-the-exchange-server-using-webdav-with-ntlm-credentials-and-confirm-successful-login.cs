using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real server details.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping actual connection.");
                return;
            }

            // Create credentials for NTLM authentication.
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Connect to Exchange using WebDAV (ExchangeClient) and verify login.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, credentials))
            {
                try
                {
                    // Attempt to retrieve mailbox information as a login test.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // If no exception was thrown, authentication succeeded.
                    Console.WriteLine("Successfully authenticated to Exchange server.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication failed: {ex.Message}");
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
