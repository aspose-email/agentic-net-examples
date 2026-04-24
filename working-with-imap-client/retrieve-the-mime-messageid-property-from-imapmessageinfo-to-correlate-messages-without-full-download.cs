using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    client.SelectFolder("INBOX");

                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo info in messages)
                    {
                        string messageId = info.MessageId;
                        Console.WriteLine($"Message UID: {info.UniqueId}, Message-ID: {messageId}");
                    }
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
