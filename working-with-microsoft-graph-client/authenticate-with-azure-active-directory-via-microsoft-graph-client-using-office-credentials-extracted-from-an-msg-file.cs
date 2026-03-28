using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(msgPath))
            {
                string userEmail = message.From.Address;

                // Dummy OAuth credentials – replace with real values.
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                Aspose.Email.Clients.ITokenProvider tokenProvider;
                try
                {
                    tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                    return;
                }

                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    client.ResourceId = userEmail; // Set the user principal name.

                    try
                    {
                        // List messages in the Inbox folder.
                        var messages = client.ListMessages("Inbox");
                        foreach (MessageInfo info in messages)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
