using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or keep as placeholders to skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Skip external calls when placeholders are detected.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Create token provider and Graph client.
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // List messages in the Inbox folder (fallback overload).
                // "Inbox" is a well‑known folder name accepted by the API.
                IList<MessageInfo> messages = client.ListMessages("Inbox");

                // Find the message that represents the new application registration blade.
                MessageInfo targetMessage = null;
                foreach (MessageInfo info in messages)
                {
                    if (info.Subject != null && info.Subject.Contains("Application registration", StringComparison.OrdinalIgnoreCase))
                    {
                        targetMessage = info;
                        break;
                    }
                }

                if (targetMessage == null)
                {
                    Console.WriteLine("No application registration message found.");
                    return;
                }

                // Fetch the full MAPI message using its ItemId.
                using (MapiMessage mapiMessage = client.FetchMessage(targetMessage.ItemId))
                {
                    Console.WriteLine("Subject: " + mapiMessage.Subject);
                    Console.WriteLine("From: " + mapiMessage.SenderName);
                    Console.WriteLine("Body:");
                    Console.WriteLine(mapiMessage.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
