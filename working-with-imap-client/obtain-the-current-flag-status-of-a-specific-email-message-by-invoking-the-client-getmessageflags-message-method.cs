using System;
using Aspose.Email;
using Aspose.Email.Clients;
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
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping network call.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Unique identifier of the message whose flags we want to inspect
                string messageUid = "1"; // replace with actual UID

                // Retrieve message information
                ImapMessageInfo messageInfo;
                try
                {
                    messageInfo = client.ListMessage(messageUid);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve message info: {ex.Message}");
                    return;
                }

                // Obtain the flags for the message
                ImapMessageFlags flags = messageInfo.Flags;
                Console.WriteLine($"Message UID {messageUid} flags: {flags}");

                // Example checks for specific flags
                Console.WriteLine($"Is Read: {flags.HasFlag(ImapMessageFlags.IsRead)}");
                Console.WriteLine($"Is Flagged: {flags.HasFlag(ImapMessageFlags.Flagged)}");
                Console.WriteLine($"Is Answered: {flags.HasFlag(ImapMessageFlags.Answered)}");
                Console.WriteLine($"Is Deleted: {flags.HasFlag(ImapMessageFlags.Deleted)}");
                Console.WriteLine($"Is Draft: {flags.HasFlag(ImapMessageFlags.Draft)}");
                Console.WriteLine($"Is Recent: {flags.HasFlag(ImapMessageFlags.Recent)}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
