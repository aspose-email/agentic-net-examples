using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";
            string uidToDelete = "12345";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || username.Contains("username") || password.Contains("password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select a folder to ensure the client is connected
                    client.SelectFolder("INBOX");

                    // Delete the message with the specified UID and commit the change immediately
                    client.DeleteMessageAsync(uidToDelete, true).GetAwaiter().GetResult();

                    Console.WriteLine($"Message with UID {uidToDelete} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
