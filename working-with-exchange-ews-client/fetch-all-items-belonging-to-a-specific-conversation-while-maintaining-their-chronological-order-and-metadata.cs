using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings – replace with your actual Exchange details
            string serviceUrl = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Identifier of the conversation to retrieve
            string conversationId = "YOUR_CONVERSATION_ID";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password" || conversationId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (implements IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Fetch all messages that belong to the specified conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);

                // Sort messages chronologically (oldest first) while preserving metadata
                List<MailMessage> orderedMessages = messages.OrderBy(m => m.Date).ToList();

                // Output basic metadata for each message
                foreach (MailMessage msg in orderedMessages)
                {
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.From}");
                    Console.WriteLine($"Date: {msg.Date}");
                    Console.WriteLine($"Size: {(msg.Body != null ? msg.Body.Length : 0)} characters");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
