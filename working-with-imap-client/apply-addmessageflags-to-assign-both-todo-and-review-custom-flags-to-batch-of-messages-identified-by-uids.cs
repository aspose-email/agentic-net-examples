using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (string.IsNullOrWhiteSpace(host) || host.Contains("example") ||
                string.IsNullOrWhiteSpace(username) || username.Contains("example") ||
                string.IsNullOrWhiteSpace(password) || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // List of message UIDs to which custom flags will be added
            List<string> uidSet = new List<string> { "1001", "1002", "1003" };

            // Combine custom flags "Todo" and "Review"
            ImapMessageFlags customFlags = ImapMessageFlags.Keyword("Todo") | ImapMessageFlags.Keyword("Review");

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.SelectFolder("INBOX");
                    client.AddMessageFlags(uidSet, customFlags);
                    Console.WriteLine("Custom flags added to specified messages.");
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
