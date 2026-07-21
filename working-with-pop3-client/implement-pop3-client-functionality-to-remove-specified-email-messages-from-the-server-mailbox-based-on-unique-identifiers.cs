using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

// Author: Example code for deleting POP3 messages based on unique IDs
class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Unique identifiers of messages to be removed
            string[] idsToDelete = new string[] { "12345", "67890" };

            // Initialize POP3 client (auto security negotiation)
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Mark each specified message for deletion
                    foreach (string uniqueId in idsToDelete)
                    {
                        pop3Client.DeleteMessage(uniqueId);
                    }

                    // Commit deletions; server will remove marked messages on session termination
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
