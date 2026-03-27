using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server configuration
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Unique identifiers of messages to delete
                List<string> idsToDelete = new List<string>
                {
                    "UID12345",
                    "UID67890"
                };

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, port, SecurityOptions.Auto))
                {
                    client.Username = username;
                    client.Password = password;

                    try
                    {
                        // Retrieve all messages from the mailbox
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        // Mark specified messages for deletion
                        foreach (Pop3MessageInfo info in messages)
                        {
                            if (idsToDelete.Contains(info.UniqueId))
                            {
                                client.DeleteMessage(info.UniqueId);
                                Console.WriteLine($"Marked for deletion: {info.Subject}");
                            }
                        }

                        // Commit deletions to the server
                        client.CommitDeletes();
                        Console.WriteLine("Deletions committed.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                        return;
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
