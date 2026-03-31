using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder POP3 server credentials
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // List all messages in the mailbox
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();

                    // Iterate through messages and delete those that match a condition
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Fetch the full message to inspect its subject
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            // Example condition: delete messages whose subject contains "DeleteMe"
                            if (message.Subject != null && message.Subject.Contains("DeleteMe"))
                            {
                                // Mark the message for deletion by sequence number
                                client.DeleteMessage(info.SequenceNumber);
                                Console.WriteLine($"Marked for deletion: {info.SequenceNumber} - {message.Subject}");
                            }
                        }
                    }

                    // Commit deletions so the server removes the marked messages
                    Console.WriteLine("Deletion commit completed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
