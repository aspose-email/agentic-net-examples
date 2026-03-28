using System;
using System.Collections.Generic;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Unique identifiers of messages to delete
            List<string> idsToDelete = new List<string> { "UID12345", "UID67890" };

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials to ensure connection is successful
                    client.ValidateCredentials();

                    // Mark each specified message for deletion
                    foreach (string uid in idsToDelete)
                    {
                        client.DeleteMessage(uid);
                        Console.WriteLine($"Marked message {uid} for deletion.");
                    }

                    // Commit deletions on the server
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
