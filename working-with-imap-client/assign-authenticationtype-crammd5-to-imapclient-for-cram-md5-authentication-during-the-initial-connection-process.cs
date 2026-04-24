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

            // Skip real connection when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Assign CRAM-MD5 authentication type
                client.AllowedAuthentication = ImapKnownAuthenticationType.CramMD5;

                // Additional operations can be performed here, e.g., client.SelectFolder("INBOX");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
