using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

public class Program
{
    public static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP credentials detected. Skipping connection.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve messages from the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
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
