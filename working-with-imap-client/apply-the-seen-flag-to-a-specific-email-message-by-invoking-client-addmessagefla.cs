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
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Apply the IsRead flag (equivalent to Seen) to the message with sequence number 1
                client.AddMessageFlags(1, ImapMessageFlags.IsRead);
                Console.WriteLine("Flag applied successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}