using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Tools.Search;

namespace DeleteEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder credentials
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping email deletion.");
                    return;
                }

                // Initialize the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve the list of messages in the folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        // Ensure there is at least one message to delete
                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found in the INBOX.");
                            return;
                        }

                        // Choose the first message as the target for deletion
                        ImapMessageInfo targetMessageInfo = messages[0];
                        string targetUniqueId = targetMessageInfo.UniqueId;

                        // Delete the specific message using its unique identifier
                        client.DeleteMessage(targetUniqueId);

                        // Optionally, commit the deletion if the server supports UIDPLUS
                        Console.WriteLine($"Message with UID '{targetUniqueId}' has been deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during IMAP operations: {ex.Message}");
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
