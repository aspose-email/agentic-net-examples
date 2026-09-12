using Aspose.Email;
using Aspose.Email.Clients.Imap;
using System;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping IMAP operations.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                // If the ImapClient version supports a KeepAlive property, enable it.
                // Otherwise, perform a NOOP command periodically to keep the connection alive.
                // client.KeepAlive = true; // Uncomment if property exists.

                // Perform a lightweight operation to establish the connection
                client.SelectFolder("INBOX");

                // Send a NOOP command to ensure the server keeps the session alive
                client.Noop();

                Console.WriteLine("IMAP connection established and kept alive.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
