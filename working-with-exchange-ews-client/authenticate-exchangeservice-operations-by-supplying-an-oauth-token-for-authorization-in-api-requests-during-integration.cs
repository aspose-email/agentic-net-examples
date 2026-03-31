using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            // OAuth 2.0 access token (replace with a real token)
            string accessToken = "your_oauth_token";

            // Guard against placeholder token to avoid real network calls during CI
            if (accessToken == "your_oauth_token")
            {
                Console.Error.WriteLine("OAuth token not provided. Skipping Exchange operation.");
                return;
            }

            // Prepare Authorization header
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + accessToken }
            };

            IEWSClient client = null;
            try
            {
                // Create EWS client with OAuth token in headers
                client = EWSClient.GetEWSClient(mailboxUri, null, null, headers);

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                return;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
