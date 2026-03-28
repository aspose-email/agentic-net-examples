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
                // Token provider for Microsoft Graph (Outlook)
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    "clientId",          // Replace with your client ID
                    "clientSecret",      // Replace with your client secret
                    "refreshToken");     // Replace with your refresh token

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Folder identifier (e.g., "Inbox")
                    string folderId = "Inbox";

                    // Collection to hold fetched messages
                    List<MailMessage> messages = new List<MailMessage>();

                    // Initial page request

                    // Loop through pages until the last page is reached
                    do
                    {
                        // Append messages from the current page

                        // If this is the last page, exit loop
                            break;

                        // Retrieve next page
                    }
                    while (true);

                    // Example output: count of retrieved messages
                    Console.WriteLine($"Total messages retrieved: {messages.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
