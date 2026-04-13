using Aspose.Email.Clients.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    // Custom async token provider that caches the OAuth token.
    public class MyAsyncTokenProvider : IAsyncTokenProvider
    {
        private readonly TokenProvider _tokenProvider;
        private OAuthToken _cachedToken;
        private readonly object _lock = new object();

        public MyAsyncTokenProvider(string requestUrl, string clientId, string clientSecret, string refreshToken)
        {
            // Obtain a TokenProvider instance for Outlook.
            _tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken);
        }

        // Retrieves the access token, optionally ignoring the cached one.
        public async Task<OAuthToken> GetAccessTokenAsync(bool ignoreExistingToken = false, CancellationToken cancellationToken = default)
        {
            if (!ignoreExistingToken && _cachedToken != null)
            {
                return _cachedToken;
            }

            // TokenProvider.GetAccessToken() is synchronous; wrap it in Task.Run.
            OAuthToken newToken = await Task.Run(() => _tokenProvider.GetAccessToken(), cancellationToken);
            lock (_lock)
            {
                _cachedToken = newToken;
            }
            return newToken;
        }

        public void Dispose()
        {
            _tokenProvider?.Dispose();
        }
    }

    public class Program
    {
        public static void Main()
        {
            try
            {
                // Replace the placeholders with actual values before running.
                string requestUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";

                // Simple guard against placeholder literals.
                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Please replace placeholder credentials with actual values.");
                    return;
                }

                // Create the async token provider.
                using (IAsyncTokenProvider tokenProvider = new MyAsyncTokenProvider(requestUrl, clientId, clientSecret, refreshToken))
                {
                    // Create OAuth credential using the async token provider.
                    OAuthNetworkCredential credential = new OAuthNetworkCredential(tokenProvider);

                    // Connect to EWS using the credential.
                    try
                    {
                        using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credential))
                        {
                            // Perform a simple operation: retrieve mailbox information.
                            ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                            Console.WriteLine("Mailbox display name: " + mailboxInfo.MailboxUri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("EWS client error: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
