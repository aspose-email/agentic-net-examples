using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Skip execution when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping IMAP connection.");
                return;
            }

            // Create the IMAP client and ensure it is disposed properly
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                // Validate credentials asynchronously (establishes the connection)
                try
                {
                    await client.ValidateCredentialsAsync(null, CancellationToken.None);
                    Console.WriteLine("IMAP connection established successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Example operation: list the first 10 messages asynchronously
                try
                {
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync(10);
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
