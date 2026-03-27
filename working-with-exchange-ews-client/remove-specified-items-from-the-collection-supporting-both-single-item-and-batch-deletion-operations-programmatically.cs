using Aspose.Email.Clients;
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize IMAP client with connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create and use the client within a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the target folder (e.g., INBOX)
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder
                    IList<ImapMessageInfo> messageInfos = new List<ImapMessageInfo>(client.ListMessages());

                    // ----- Single-item deletion -----
                    if (messageInfos.Count > 0)
                    {
                        // Delete the first message by its unique identifier and commit the deletion immediately
                        string firstMessageUid = messageInfos[0].UniqueId;
                        client.DeleteMessage(firstMessageUid, true);
                        Console.WriteLine("Deleted single message with UID: " + firstMessageUid);
                    }

                    // ----- Batch deletion -----
                    // Determine a batch of messages to delete (e.g., first three messages)
                    IList<ImapMessageInfo> batchToDelete = new List<ImapMessageInfo>();
                    for (int i = 0; i < Math.Min(3, messageInfos.Count); i++)
                    {
                        batchToDelete.Add(messageInfos[i]);
                    }

                    if (batchToDelete.Count > 0)
                    {
                        // Delete the batch of messages and commit the deletions
                        client.DeleteMessages(batchToDelete, true);
                        Console.WriteLine("Deleted batch of " + batchToDelete.Count + " messages.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any errors to the error stream without crashing the application
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
