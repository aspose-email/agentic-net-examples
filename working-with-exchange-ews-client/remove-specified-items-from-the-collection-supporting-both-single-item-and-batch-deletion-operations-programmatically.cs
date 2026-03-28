using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Server connection settings (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Initialize and connect the IMAP client
                try
                {
                    using (ImapClient client = new ImapClient(host, port, username, password))
                    {
                        client.SecurityOptions = SecurityOptions.SSLImplicit;

                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve all messages in the folder
                        ImapMessageInfoCollection messages = client.ListMessages();
                        Console.WriteLine($"Total messages in INBOX: {messages.Count}");

                        // ----- Single-item deletion -----
                        if (messages.Count > 0)
                        {
                            // Delete the first message by its unique identifier
                            ImapMessageInfo firstMessage = messages[0];
                            client.DeleteMessage(firstMessage.UniqueId);
                            client.CommitDeletes();
                            Console.WriteLine($"Deleted single message UID: {firstMessage.UniqueId}");
                        }

                        // ----- Batch deletion -----
                        if (messages.Count > 2)
                        {
                            // Prepare a list of messages to delete (e.g., second and third messages)
                            List<ImapMessageInfo> batchToDelete = new List<ImapMessageInfo>
                            {
                                messages[1],
                                messages[2]
                            };

                            // Delete the batch and commit immediately
                            client.DeleteMessages(batchToDelete, true);
                            Console.WriteLine("Deleted batch of messages (UIDs: {0}, {1})",
                                batchToDelete[0].UniqueId, batchToDelete[1].UniqueId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle connection or operation errors
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
