using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select the folder from which flags should be cleared
                client.SelectFolder("INBOX");

                // Retrieve all messages in the selected folder
                IList<ImapMessageInfo> messages = client.ListMessages();

                // Combine all possible flags
                ImapMessageFlags allFlags = ImapMessageFlags.Answered |
                                            ImapMessageFlags.Deleted |
                                            ImapMessageFlags.Draft |
                                            ImapMessageFlags.Flagged |
                                            ImapMessageFlags.IsRead |
                                            ImapMessageFlags.Recent;

                // Remove all flags from each message
                foreach (ImapMessageInfo info in messages)
                {
                    List<ImapMessageInfo> singleMessage = new List<ImapMessageInfo> { info };
                    client.RemoveMessageFlags(singleMessage, allFlags);
                }

                Console.WriteLine("All flag attributes have been cleared.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
