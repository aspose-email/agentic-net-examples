using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder OAuth credentials – replace with real values.
            const string clientId = "YOUR_CLIENT_ID";
            const string clientSecret = "YOUR_CLIENT_SECRET";
            const string refreshToken = "YOUR_REFRESH_TOKEN";
            const string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";

            // Guard against running with placeholder credentials.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("OAuth credentials are placeholders. Skipping EWS operations.");
                return;
            }

            // Acquire OAuth token.
            using (TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken))
            {
                // GetAccessToken returns OAuthToken; use its Token property.
                OAuthToken oauthToken = tokenProvider.GetAccessToken();
                string accessToken = oauthToken.Token;

                // Optional: force token renewal if needed.
                // OAuthToken refreshedToken = tokenProvider.GetAccessToken(true);
                // accessToken = refreshedToken.Token;

                // Create asynchronous EWS client using the access token.
                // The token is supplied via NetworkCredential (token as user, empty password).
                using (IAsyncEwsClient ewsClient = await EWSClient.GetEwsClientAsync(
                    mailboxUri,
                    new NetworkCredential(accessToken, string.Empty),
                    proxy: null,
                    cancellationToken: CancellationToken.None))
                {
                    // Retrieve mailbox information.
                    ExchangeMailboxInfo mailboxInfo = await ewsClient.GetMailboxInfoAsync();

                    // Example: output the Inbox URI (no DisplayName property used).
                    Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
