using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main()
    {
        try
        {
            // -----------------------------------------------------------------
            // Configuration – replace with real values for a functional run.
            // -----------------------------------------------------------------
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string userEmail = "user@example.com";
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against placeholder values
            if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("YOUR_"))
                throw new ArgumentException("clientId is not set.");
            if (string.IsNullOrWhiteSpace(clientSecret) || clientSecret.Contains("YOUR_"))
                throw new ArgumentException("clientSecret is not set.");
            if (string.IsNullOrWhiteSpace(refreshToken) || refreshToken.Contains("YOUR_"))
                throw new ArgumentException("refreshToken is not set.");

            // -----------------------------------------------------------------
            // Acquire an OAuth access token.
            // -----------------------------------------------------------------
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.GetInstance(clientId, clientSecret, refreshToken, null);
            OAuthToken token = tokenProvider.GetAccessToken();
            string accessToken = token.Token;

            // -----------------------------------------------------------------
            // Build credentials for EWS – username is the email, password is the token.
            // -----------------------------------------------------------------
            NetworkCredential credentials = new NetworkCredential(userEmail, accessToken);

            // -----------------------------------------------------------------
            // Create an asynchronous EWS client.
            // -----------------------------------------------------------------
            using (IAsyncEwsClient ewsClient = await EWSClient.GetEwsClientAsync(mailboxUri, credentials))
            {
                // Example operation: fetch mailbox information (async version).
                var mailboxInfo = await ewsClient.GetMailboxInfoAsync();

                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");

                // -----------------------------------------------------------------
                // Token renewal example – re-acquire token before it expires.
                // -----------------------------------------------------------------
                OAuthToken renewedToken = tokenProvider.GetAccessToken();
                if (!string.Equals(accessToken, renewedToken.Token, StringComparison.Ordinal))
                {
                    ewsClient.Credentials = new NetworkCredential(userEmail, renewedToken.Token);
                    Console.WriteLine("OAuth token renewed and client credentials updated.");
                }

                // Additional EWS operations can be performed here using ewsClient.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
