using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services connection parameters
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip external calls
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = "ConversationMessages";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the EWS client
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Get the Inbox folder URI
                string inboxUri = ewsClient.GetMailboxInfo().InboxUri;

                // Find all conversations in the Inbox
                ExchangeConversation[] conversations = ewsClient.FindConversations(inboxUri);
                foreach (ExchangeConversation conversation in conversations)
                {
                    // Retrieve all messages belonging to the conversation
                    MailMessageCollection messages = ewsClient.FetchConversationMessages(conversation.ConversationId);
                    Console.WriteLine($"Conversation Id: {conversation.ConversationId}, Total messages: {messages.Count}");

                    foreach (MailMessage message in messages)
                    {
                        // Output basic metadata
                        Console.WriteLine($"  Subject: {message.Subject}");
                        Console.WriteLine($"  From: {message.From}");
                        Console.WriteLine($"  Date: {message.Date}");

                        // Build a safe file name for the message
                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : MakeFileNameSafe(message.Subject);
                        string filePath = Path.Combine(outputDir,
                            $"{conversation.ConversationId}_{safeSubject}_{Guid.NewGuid()}.eml");

                        try
                        {
                            // Save the message to a file
                            message.Save(filePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message to '{filePath}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper method to replace invalid filename characters with an underscore
    private static string MakeFileNameSafe(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
