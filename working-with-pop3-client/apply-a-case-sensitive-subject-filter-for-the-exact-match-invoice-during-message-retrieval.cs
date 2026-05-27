using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI environments
            string host = "pop3.example.com";
            string username = "user";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to authenticate POP3 client: {ex.Message}");
                    return;
                }

                // Retrieve all messages
                Pop3MessageInfoCollection allMessages;
                try
                {
                    allMessages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                    return;
                }

                // Apply a case‑sensitive exact subject filter for "Invoice"
                var filteredMessages = allMessages
                    .Where(info => string.Equals(info.Subject, "Invoice", StringComparison.Ordinal))
                    .ToList();

                // Display the subjects of the filtered messages
                foreach (var info in filteredMessages)
                {
                    Console.WriteLine($"Message UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
