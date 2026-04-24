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
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string uniqueId = "12345"; // The unique ID of the target message

            // Skip execution when using placeholder credentials/host
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Define the custom flag "Important"
                    ImapMessageFlags importantFlag = ImapMessageFlags.Keyword("Important");

                    // Add the custom flag to the message identified by its unique ID
                    client.AddMessageFlags(uniqueId, importantFlag);

                    Console.WriteLine($"Custom flag 'Important' added to message with UID {uniqueId}.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while adding flag: {ex.Message}");
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
