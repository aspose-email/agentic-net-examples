using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize the IMAP client with SSL
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Activate detailed activity logging
                client.EnableLogger = true;
                client.LogFileName = "imap_activity.log";
                client.UseDateInLogFileName = true;

                // Perform a simple operation to trigger connection and logging
                client.SelectFolder("INBOX");
                Console.WriteLine("Connected to IMAP server and selected INBOX.");

                // List messages in the selected folder
                var messages = client.ListMessages();
                Console.WriteLine($"Total messages in INBOX: {messages.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
