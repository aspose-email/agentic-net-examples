using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping connection.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Set a connection checkup period (in milliseconds) to enable automatic reconnection attempts
                client.ConnectionCheckupPeriod = 60000; // 1 minute

                // Optional: enable logging for troubleshooting
                client.EnableLogger = true;

                // Perform a lightweight operation to establish the connection
                client.SelectFolder("INBOX");
                Console.WriteLine("Connected to IMAP server and INBOX selected successfully.");
            }
        }
        catch (ImapException ex)
        {
            Console.Error.WriteLine($"IMAP error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
