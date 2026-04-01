using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials.
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder.
                    ImapMessageInfoCollection messages = client.ListMessages();

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Process the first message as a sample.
                    ImapMessageInfo firstInfo = messages[0];
                    string uniqueId = firstInfo.UniqueId;

                    // Fetch the full message.
                    MailMessage message = client.FetchMessage(uniqueId);
                    Console.WriteLine("Subject: " + message.Subject);

                    // Update flags: mark the message as read.
                    client.AddMessageFlags(uniqueId, ImapMessageFlags.IsRead);
                    Console.WriteLine("Message flagged as read.");

                    // Delete the message and commit the deletion.
                    client.DeleteMessage(uniqueId);
                    Console.WriteLine("Message deleted.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("IMAP operation error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
