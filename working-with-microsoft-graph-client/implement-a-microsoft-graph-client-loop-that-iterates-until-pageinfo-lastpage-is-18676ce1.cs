using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            // Create a token provider (placeholder credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize the Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // Identifier of the folder to list messages from (e.g., Inbox)
                string folderId = "Inbox";

                // Collection to hold all retrieved messages
                List<MessageInfo> allMessages = new List<MessageInfo>();

                // Retrieve messages without paging (fallback approach)
                MessageInfoCollection messagePage = graphClient.ListMessages(folderId);

                // Append retrieved items to the list
                foreach (MessageInfo messageInfo in messagePage)
                {
                    allMessages.Add(messageInfo);
                }

                // Example output
                Console.WriteLine("Total messages retrieved: " + allMessages.Count);
                foreach (MessageInfo info in allMessages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}