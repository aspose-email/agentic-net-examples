using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server configuration
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP validation.");
                return;
            }

            // Periodically validate credentials
            while (true)
            {
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    try
                    {
                        bool isValid = client.ValidateCredentials();
                        if (isValid)
                        {
                            Console.WriteLine($"[{DateTime.Now}] IMAP credentials are valid.");
                        }
                        else
                        {
                            Console.Error.WriteLine($"[{DateTime.Now}] IMAP credentials validation failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"[{DateTime.Now}] Validation error: {ex.Message}");
                    }
                }

                // Wait before the next validation (e.g., 5 minutes)
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
