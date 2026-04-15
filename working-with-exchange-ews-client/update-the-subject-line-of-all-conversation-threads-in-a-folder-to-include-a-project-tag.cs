using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client inside a using block to ensure disposal
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Specify the target folder (e.g., Inbox). Adjust as needed.
                    string targetFolderUri = client.MailboxInfo.InboxUri;

                    // Retrieve information about the target folder
                    ExchangeFolderInfo folderInfo = client.GetFolderInfo(targetFolderUri);

                    // Find all conversation threads in the folder
                    ExchangeConversation[] conversations = client.FindConversations(folderInfo.Uri);

                    foreach (ExchangeConversation conversation in conversations)
                    {
                        // Iterate through each message identifier in the conversation
                        foreach (string messageUri in conversation.ItemIds)
                        {
                            try
                            {
                                // Fetch the original message
                                MailMessage originalMessage = client.FetchMessage(messageUri);

                                // Prepare the new subject with the project tag
                                string projectTag = "[Project]";

                                // Skip external calls when placeholder credentials are used
                                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                                {
                                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                                    return;
                                }

                                string newSubject = originalMessage.Subject != null && originalMessage.Subject.StartsWith(projectTag)
                                    ? originalMessage.Subject
                                    : $"{projectTag} {originalMessage.Subject}";

                                // Skip if the subject already contains the tag
                                if (originalMessage.Subject == newSubject)
                                    continue;

                                // Update the subject
                                originalMessage.Subject = newSubject;

                                // Delete the original message
                                client.DeleteItem(messageUri, new DeletionOptions(DeletionType.MoveToDeletedItems));

                                // Append the updated message back to the same folder
                                client.AppendMessage(folderInfo.Uri, originalMessage);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to process message '{messageUri}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
