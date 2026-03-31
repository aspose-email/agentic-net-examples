using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values when available.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Guard against executing external calls with placeholder credentials.
                if (clientId == "clientId" || clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken" || defaultEmail == "user@example.com")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                    return;
                }

                // Initialize Gmail client.
                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

                try
                {
                    // Retrieve the list of messages.
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();

                    // Output basic information for each message.
                    foreach (GmailMessageInfo messageInfo in messages)
                    {
                        // GmailMessageInfo does not have a Subject property; use Id as an identifier.
                        Console.WriteLine($"Message Id: {messageInfo.Id}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors related to Gmail client operations.
                    Console.Error.WriteLine($"Gmail client error: {ex.Message}");
                }
                finally
                {
                    // Dispose the client if it implements IDisposable.
                    if (gmailClient is IDisposable disposableClient)
                    {
                        disposableClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
