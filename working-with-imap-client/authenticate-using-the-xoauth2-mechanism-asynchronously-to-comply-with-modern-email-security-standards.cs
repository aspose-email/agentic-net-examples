using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder values – replace with real credentials for actual use.
            string host = "imap.example.com";
            string username = "user@example.com";
            string accessToken = "YOUR_ACCESS_TOKEN";

            // Skip real network call when placeholders are detected.
            if (host.Contains("example.com") || accessToken.StartsWith("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping XOAUTH2 authentication.");
                return;
            }

            // Create IMAP client using XOAUTH2 (useOAuth = true).
            using (ImapClient client = new ImapClient(host, username, accessToken, true))
            {
                try
                {
                    // Asynchronously validate credentials.
                    bool isValid = await client.ValidateCredentialsAsync();
                    Console.WriteLine(isValid
                        ? "XOAUTH2 authentication succeeded."
                        : "XOAUTH2 authentication failed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
