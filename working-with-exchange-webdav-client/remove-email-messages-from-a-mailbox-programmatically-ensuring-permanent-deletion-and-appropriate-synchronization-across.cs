using Aspose.Email;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    // Author: Aspose.Email example - permanently delete all messages from a POP3 mailbox.
    static async Task Main()
    {
        try
        {
            // POP3 server connection settings.
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the POP3 client.
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Attempt to delete all messages in the mailbox.
                // The POP3 server marks messages as deleted; they are removed when the session ends.
                await pop3Client.DeleteMessagesAsync();

                Console.WriteLine("All messages have been marked for permanent deletion.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
