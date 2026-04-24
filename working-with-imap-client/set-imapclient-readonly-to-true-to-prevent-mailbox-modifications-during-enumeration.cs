using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapReadOnlyExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and dispose the ImapClient safely
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Prevent any modifications to the mailbox
                        client.ReadOnly = true;

                        // Select the INBOX folder (this will also establish the connection)
                        client.SelectFolder("INBOX");

                        // Enumerate messages in the selected folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        foreach (ImapMessageInfo info in messages)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
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
}
