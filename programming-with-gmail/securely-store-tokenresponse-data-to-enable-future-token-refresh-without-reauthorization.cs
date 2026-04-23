using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailTokenStorageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values for actual use.
                string host = "imap.gmail.com";
                string username = "user@example.com";
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";

                // Guard against placeholder values to avoid unintended network calls.
                if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                    return;
                }

                // Obtain a TokenProvider for Google (Gmail) using the refresh token.
                TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

                // Retrieve the OAuth access token.
                OAuthToken oauthToken = tokenProvider.GetAccessToken();

                // Store the access token securely (simple file storage for demonstration).
                string tokenFilePath = "token.dat";
                try
                {
                    string directory = Path.GetDirectoryName(tokenFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.WriteAllText(tokenFilePath, oauthToken.Token);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to store token: {ioEx.Message}");
                    return;
                }

                // Configure the ImapClient to use the TokenProvider for authentication.
                using (ImapClient imapClient = new ImapClient(host, username, tokenProvider))
                {
                    try
                    {
                        imapClient.ValidateCredentials();
                        Console.WriteLine("IMAP client authenticated using stored token.");
                    }
                    catch (Exception clientEx)
                    {
                        Console.Error.WriteLine($"IMAP connection failed: {clientEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
