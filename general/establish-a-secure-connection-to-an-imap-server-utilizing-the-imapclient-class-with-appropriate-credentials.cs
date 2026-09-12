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
            // Placeholder connection details
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Create the IMAP client with secure SSL implicit mode
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Validate credentials (establishes a secure connection)
                    client.ValidateCredentials();
                    Console.WriteLine("Successfully connected and authenticated to the IMAP server.");
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
