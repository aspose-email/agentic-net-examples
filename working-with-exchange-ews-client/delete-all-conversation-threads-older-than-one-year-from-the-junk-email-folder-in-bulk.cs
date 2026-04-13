using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create EWS client (replace with actual server URI and credentials)
            try
            {
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Get the Junk Email folder information
                    ExchangeFolderInfo junkFolderInfo = client.GetFolderInfo("junkemail");
                    string junkFolderUri = junkFolderInfo.Uri;

                    // Retrieve all conversations in the Junk Email folder
                    ExchangeConversation[] conversations = client.FindConversations(junkFolderUri);

                    // Define cutoff date (one year ago)
                    DateTime cutoffDate = DateTime.Now.AddYears(-1);

                    // Delete conversations older than the cutoff date
                    foreach (ExchangeConversation conversation in conversations)
                    {
                        // Use GlobalLastDeliveryTime to determine the most recent message in the conversation
                        if (conversation.GlobalLastDeliveryTime < cutoffDate)
                        {
                            client.DeleteConversationItems(conversation.ConversationId, junkFolderUri);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
