using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            bool useSsl = true;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Validate connection by selecting a folder (no explicit Connect call)
                    client.SelectFolder("INBOX");

                    // Unique identifier of the target message
                    string uniqueId = "12345";

                    // Combine custom flags "ProjectA" and "Urgent"
                    ImapMessageFlags customFlags = ImapMessageFlags.Keyword("ProjectA") |
                                                   ImapMessageFlags.Keyword("Urgent");

                    // Add the custom flags to the message
                    client.AddMessageFlagsAsync(uniqueId, customFlags).GetAwaiter().GetResult();

                    Console.WriteLine("Custom flags added successfully.");
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
