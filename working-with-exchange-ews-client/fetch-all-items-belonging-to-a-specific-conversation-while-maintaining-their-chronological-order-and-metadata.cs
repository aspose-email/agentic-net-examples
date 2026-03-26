using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox URI and credentials (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Get the Inbox folder URI
                    string inboxFolderUri = client.MailboxInfo.InboxUri;

                    // Find all conversations in the Inbox folder
                    ExchangeConversation[] conversations = client.FindConversations(inboxFolderUri);
                    if (conversations == null || conversations.Length == 0)
                    {
                        Console.WriteLine("No conversations found in the Inbox.");
                        return;
                    }

                    // For demonstration, select the first conversation (or filter by topic)
                    ExchangeConversation targetConversation = conversations[0];
                    Console.WriteLine($"Processing conversation: {targetConversation.ConversationTopic}");

                    // Fetch all messages belonging to the selected conversation
                    MailMessageCollection messages = client.FetchConversationMessages(targetConversation.ConversationId);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found for the conversation.");
                        return;
                    }

                    // Sort messages by the Date property to maintain chronological order
                    // Simple bubble sort for illustration (avoid LINQ to stay within allowed APIs)
                    for (int i = 0; i < messages.Count - 1; i++)
                    {
                        for (int j = 0; j < messages.Count - i - 1; j++)
                        {
                            MailMessage msgA = messages[j];
                            MailMessage msgB = messages[j + 1];
                            if (msgA.Date > msgB.Date)
                            {
                                // Swap
                                messages.RemoveAt(j);
                                messages.Insert(j, msgB);
                                messages.RemoveAt(j + 1);
                                messages.Insert(j + 1, msgA);
                            }
                        }
                    }

                    // Output metadata for each message
                    foreach (MailMessage message in messages)
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + (message.From != null ? message.From.ToString() : "N/A"));
                        Console.WriteLine("Date: " + message.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}