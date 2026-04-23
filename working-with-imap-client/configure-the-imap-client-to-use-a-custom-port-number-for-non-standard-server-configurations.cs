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
            // Configuration parameters
            string host = "imap.example.com";
            int customPort = 993; // non‑standard port
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping IMAP client connection.");
                return;
            }

            try
            {
                // Initialize ImapClient with custom port
                using (ImapClient client = new ImapClient(host, customPort, username, password))
                {
                    // Optional: set security options if required
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Demonstrate that the client is configured with the custom port
                    Console.WriteLine($"IMAP client configured for host '{client.Host}' on custom port {client.Port}.");
                }
            }
            catch (ImapException imapEx)
            {
                Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
