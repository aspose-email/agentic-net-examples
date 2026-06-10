using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Mark all messages for deletion
                    client.DeleteMessagesAsync().GetAwaiter().GetResult();

                    // Cancel the pending deletions
                    client.UndeleteMessages();

                    // Optionally commit deletions if you wanted to keep them:
                    // client.CommitDeletesAsync().GetAwaiter().GetResult();
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
