using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration (replace with real values)
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Equals("imap.example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("user@example.com", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            const int maxAttempts = 3;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    // Create and use the ImapClient inside a using block to ensure disposal
                    using (ImapClient client = new ImapClient(host, username, password))
                    {
                        // Example operation: select the INBOX folder
                        client.SelectFolder("INBOX");
                        Console.WriteLine("Connected and INBOX selected successfully.");
                    }

                    // If we reach this point, the operation succeeded; exit the retry loop
                    break;
                }
                catch (ImapException imapEx)
                {
                    // Check if the exception is a timeout; otherwise treat as fatal
                    bool isTimeout = imapEx.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (!isTimeout || attempt == maxAttempts)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                        return;
                    }

                    // Timeout occurred – retry after logging
                    Console.WriteLine($"Timeout detected (attempt {attempt}/{maxAttempts}). Retrying...");
                }
                catch (Exception ex)
                {
                    // Any other unexpected exception is treated as fatal
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
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
