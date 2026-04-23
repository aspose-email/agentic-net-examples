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
            string username = "user";
            string password = "password";

            // Skip external calls when placeholder credentials are detected
            if (host.Contains("example.com") || username == "user")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");
                    client.Noop();

                    // List messages in the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();
                    client.Noop();

                    // If there is at least one message, fetch and delete it
                    if (messages != null && messages.Count > 0)
                    {
                        ImapMessageInfo firstInfo = messages[0];

                        // Fetch the first message
                        MailMessage firstMessage = client.FetchMessage(firstInfo.UniqueId);
                        client.Noop();

                        // Delete the fetched message (move to trash)
                        client.DeleteMessage(firstInfo.UniqueId, true);
                        client.Noop();
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
