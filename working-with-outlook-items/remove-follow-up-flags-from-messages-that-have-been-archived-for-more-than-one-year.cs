using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution if they are placeholders.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (host.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client within a using block to ensure proper disposal.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the archive folder (adjust the folder name if needed).
                    client.SelectFolder("Archive");

                    // Retrieve all messages in the archive folder.
                    ImapMessageInfoCollection messages = client.ListMessages();

                    DateTime cutoffDate = DateTime.Now.AddYears(-1);

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Check if the message is older than one year and has the Flagged (follow‑up) flag.
                        if (info.InternalDate < cutoffDate && info.Flags.HasFlag(ImapMessageFlags.Flagged))
                        {
                            // Remove the Flagged flag from the message.
                            client.RemoveMessageFlags(info.SequenceNumber, ImapMessageFlags.Flagged);
                            Console.WriteLine($"Removed follow‑up flag from message UID {info.UniqueId} (Subject: {info.Subject})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
