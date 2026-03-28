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
            // Create token provider (replace with real credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId", "clientSecret", "refreshToken");

            // Initialize Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
            {
                // Folder identifier (e.g., Inbox)
                string folderId = "Inbox";

                // Collection to hold all MessageInfo objects
                List<MessageInfo> messages = new List<MessageInfo>();

                // Initial page request

                // Iterate through pages until the last page is reached
                do
                {
                    // Append current page items

                    // Exit if this is the last page
                        break;

                } while (true);

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
