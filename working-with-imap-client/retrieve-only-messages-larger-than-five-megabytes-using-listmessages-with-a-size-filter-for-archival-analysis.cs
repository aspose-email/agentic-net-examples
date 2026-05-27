using Aspose.Email;
using System;
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

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                // Select the folder to search in
                client.SelectFolder("INBOX");

                // IMAP search criteria for messages larger than 5 MB (5 * 1024 * 1024 bytes)
                string sizeCriteria = "LARGER 5242880";

                // Retrieve messages matching the size filter
                ImapMessageInfoCollection messages = client.ListMessages(sizeCriteria);

                Console.WriteLine($"Found {messages.Count} messages larger than 5 MB:");
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Size: {info.Size} bytes, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
