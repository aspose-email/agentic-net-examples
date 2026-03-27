using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphExample
{
    class Program
    {
        // Entry point with async support
        static async Task Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Create a token provider (placeholder credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    clientId: "your-client-id",
                    clientSecret: "your-client-secret",
                    refreshToken: "your-refresh-token");

                // Tenant identifier (placeholder)
                string tenantId = "your-tenant-id";

                // Use the Graph client inside a using block to ensure disposal
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Folder identifier – using the well‑known "Inbox" folder name as a placeholder
                    string inboxFolderId = "Inbox";

                    // List all messages in the Inbox folder
                    MessageInfoCollection messages = graphClient.ListMessages(inboxFolderId);

                    // Iterate and output only unread messages
                    foreach (MessageInfo message in messages)
                    {
                        // Guard against missing properties – check IsRead if available
                        // If the property does not exist, treat the message as unread by default
                        bool isRead = false;
                        try
                        {
                            // Some versions expose IsRead; use reflection as a safe fallback
                            var prop = message.GetType().GetProperty("IsRead");
                            if (prop != null && prop.PropertyType == typeof(bool))
                            {
                                isRead = (bool)prop.GetValue(message);
                            }
                        }
                        catch
                        {
                            // Ignore any reflection errors and keep default isRead = false
                        }

                        if (!isRead)
                        {
                            Console.WriteLine($"Unread: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any unexpected errors to the error stream
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}