using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip real network calls if defaults are used.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping IMAP operations because placeholder credentials are detected.");
                return;
            }

            // Connect to the IMAP server.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to authenticate: {ex.Message}");
                    return;
                }

                // Retrieve all messages in the selected folder (INBOX by default).
                ImapMessageInfoCollection messages = client.ListMessages();

                foreach (ImapMessageInfo info in messages)
                {
                    // Identify messages that have been replied to (Answered flag set).
                    if (info.Answered)
                    {
                        try
                        {
                            // Remove the follow‑up flag (Flagged) from the message.
                            client.RemoveMessageFlags(info.SequenceNumber, ImapMessageFlags.Flagged);
                            Console.WriteLine($"Removed follow‑up flag from message UID {info.UniqueId}.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to modify flags for message UID {info.UniqueId}: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
