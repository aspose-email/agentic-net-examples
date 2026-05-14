using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholder credentials are used
            if (host.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            // Create and use the POP3 client safely
            try
            {
                using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
                {
                    // Attempt to validate credentials (this will trigger a connection)
                    client.ValidateCredentials();

                    // Example operation: get mailbox info
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Message count: {mailboxInfo.MessageCount}, Occupied size: {mailboxInfo.OccupiedSize}");
                }
            }
            catch (Pop3Exception ex)
            {
                // Examine exception details
                Console.Error.WriteLine($"POP3 error occurred: {ex.Message}");
                string? errorDetails = ex.ErrorDetails?.ToString();
                if (!string.IsNullOrEmpty(errorDetails))
                {
                    Console.Error.WriteLine($"Additional details: {errorDetails}");
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
