using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are used.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping operation.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the folder containing the message.
                    client.SelectFolder("INBOX");

                    // Example sequence numbers to modify. Replace with actual IDs as needed.
                    List<int> sequenceNumbers = new List<int> { 1 };

                    // Combine the IsRead (Seen) and Flagged flags.
                    ImapMessageFlags flagsToRemove = ImapMessageFlags.BitwiseOr(ImapMessageFlags.IsRead, ImapMessageFlags.Flagged);

                    // Remove the specified flags from the messages.
                    client.RemoveMessageFlags(sequenceNumbers, flagsToRemove);
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
