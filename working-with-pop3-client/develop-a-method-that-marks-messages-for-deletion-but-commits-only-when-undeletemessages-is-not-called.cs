using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 server detected. Skipping network operations.");
                return;
            }

            // Create POP3 client using synchronous constructor (no async token provider needed)
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Mark all messages for deletion (server marks but does not commit yet)
                    client.DeleteMessages();

                    // Set to true to undo deletions, false to commit them
                    bool undoDeletions = false;

                    if (undoDeletions)
                    {
                        // Unmark previously marked messages
                        client.UndeleteMessages();
                        Console.WriteLine("Deletions have been undone.");
                    }
                    else
                    {
                        // Commit the deletions to the server
                        Console.WriteLine("Deletions have been committed.");
                    }
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
