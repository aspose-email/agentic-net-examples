using Aspose.Email;
using System;
using System.Threading;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        try
        {
            // Connection settings (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string messageId = "YOUR_MESSAGE_UID";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password" || messageId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize ImapClient
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.Username = username;
                imapClient.Password = password;
                imapClient.SecurityOptions = SecurityOptions.SSLImplicit;

                try
                {
                    // Asynchronously delete the message permanently (commitNow = true)
                    await imapClient.DeleteMessageAsync(messageId, true, CancellationToken.None);
                    Console.WriteLine("Message deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting message: {ex.Message}");
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
