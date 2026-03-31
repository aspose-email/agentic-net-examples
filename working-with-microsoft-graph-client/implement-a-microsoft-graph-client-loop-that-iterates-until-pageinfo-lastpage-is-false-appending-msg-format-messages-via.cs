using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string tenantId = "YOUR_TENANT_ID";

                // Guard against placeholder values to avoid external calls during CI.
                if (clientId.StartsWith("YOUR_") ||
                    clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_") ||
                    tenantId.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph client execution.");
                    return;
                }

                // Create token provider (3‑argument overload is the verified one).
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                Aspose.Email.Clients.ITokenProvider iTokenProvider = tokenProvider;

                // Initialize Graph client.
                IGraphClient client = GraphClient.GetClient(iTokenProvider, tenantId);
                using (client)
                {
                    try
                    {
                        // Folder identifier (e.g., inbox). Adjust as needed.
                        string folderId = "inbox";

                        // Collection to hold all retrieved messages.
                        List<MessageInfo> allMessages = new List<MessageInfo>();

                        // Initial page request without paging info (null) and without query.
                        GraphMessagePageInfo pageInfo = client.ListMessages(folderId, null, null);

                        // Loop until the last page is reached.
                        while (!pageInfo.LastPage)
                        {
                            // Append the items from the current page.
                            allMessages.AddRange(pageInfo.Items);

                            // Request the next page using the current pageInfo.
                            pageInfo = client.ListMessages(folderId, pageInfo, null);
                        }

                        // Append items from the final page (where LastPage == true).
                        allMessages.AddRange(pageInfo.Items);

                        Console.WriteLine($"Total messages retrieved: {allMessages.Count}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                        // No rethrow – graceful exit.
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
