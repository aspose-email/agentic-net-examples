using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls in CI.
            if (host.Contains("example") || username.Contains("example") || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            List<string> senderFilters = new List<string> { "alice@example.com", "bob@example.com" };
            List<MailMessage> matchingMessages = GetMessagesBySenders(host, port, username, password, senderFilters);

            Console.WriteLine($"Found {matchingMessages.Count} message(s) from specified senders.");
            foreach (MailMessage msg in matchingMessages)
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.From}");
                Console.WriteLine();
                msg.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static List<MailMessage> GetMessagesBySenders(string host, int port, string username, string password, List<string> senderEmails)
    {
        List<MailMessage> result = new List<MailMessage>();

        // Create and connect POP3 client.
        using (Pop3Client client = new Pop3Client())
        {
            try
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                client.ValidateCredentials();

                // Retrieve list of message infos.
                Pop3MessageInfoCollection infos = client.ListMessages();

                foreach (Pop3MessageInfo info in infos)
                {
                    // Fetch full message.
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Check if any of the sender addresses match the filter list.
                        foreach (string sender in senderEmails)
                        {
                            if (message.From != null && string.Equals(message.From.Address, sender, StringComparison.OrdinalIgnoreCase))
                            {
                                // Clone the message to keep it after disposing the client.
                                MailMessage cloned = message.Clone() as MailMessage;
                                result.Add(cloned);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                return result;
            }
        }

        return result;
    }
}
