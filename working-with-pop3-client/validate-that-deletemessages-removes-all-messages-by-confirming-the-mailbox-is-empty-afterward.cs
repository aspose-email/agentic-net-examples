using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            const string host = "pop3.example.com";
            const string username = "username";
            const string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                return;
            }

            // Create POP3 client (no explicit Connect method required)
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Delete all messages in the mailbox
                    client.DeleteMessages();

                    // Verify that the mailbox is now empty
                    int remaining = client.GetMessageCount();
                    if (remaining == 0)
                    {
                        Console.WriteLine("All messages successfully deleted. Mailbox is empty.");
                    }
                    else
                    {
                        Console.Error.WriteLine($"Deletion incomplete. {remaining} message(s) remain in the mailbox.");
                    }
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
