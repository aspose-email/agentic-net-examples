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
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            using (ImapClient client = new ImapClient(host, 993, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the folder containing the message (optional, many methods auto‑select)
                    client.SelectFolder("INBOX");

                    // UID of the message whose Deleted flag should be cleared
                    string messageUid = "123"; // replace with actual UID

                    // Remove the Deleted flag
                    client.RemoveMessageFlags(messageUid, ImapMessageFlags.Deleted);
                    Console.WriteLine($"Deleted flag removed from message UID {messageUid}.");
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
