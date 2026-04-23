using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials check
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";

                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Skipping execution due to placeholder credentials.");
                    return;
                }

                using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
                {
                    try
                    {
                        client.Username = username;
                        client.Password = password;
                        client.SelectFolder("INBOX");

                        // Retrieve all messages in the selected folder
                        ImapMessageInfoCollection allMessages = client.ListMessages();

                        List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                        foreach (ImapMessageInfo messageInfo in allMessages)
                        {
                            // Delete messages whose size exceeds 10 MB (10 * 1024 * 1024 bytes)
                            if (messageInfo.Size > 10 * 1024 * 1024)
                            {
                                messagesToDelete.Add(messageInfo);
                            }
                        }

                        if (messagesToDelete.Count > 0)
                        {
                            // Delete and commit the deletions
                            client.DeleteMessages(messagesToDelete, true);
                            Console.WriteLine($"{messagesToDelete.Count} messages deleted.");
                        }
                        else
                        {
                            Console.WriteLine("No messages with attachments larger than 10 MB found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during IMAP operations: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
