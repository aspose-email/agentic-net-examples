using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip real connection when placeholders are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder IMAP credentials detected. Skipping connection.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the selected folder
                    ImapMessageInfoCollection messagesInfo = client.ListMessages();

                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch each full message using its unique identifier
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        using (message)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {message.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
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
