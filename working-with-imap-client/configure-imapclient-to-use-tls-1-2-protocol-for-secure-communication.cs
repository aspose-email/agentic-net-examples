using Aspose.Email.Clients.Base;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

public class Program
{
    public static void Main()
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Configure the client to use only TLS 1.2
                    client.SupportedEncryption = EncryptionProtocols.Tls12;
                    Console.WriteLine("ImapClient configured to use TLS 1.2.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error configuring ImapClient: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
