using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Retrieve message list
                Pop3MessageInfoCollection messageInfos = client.ListMessages();

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    // Fetch full message for each entry
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        string from = message.From.Count > 0 ? message.From[0].Address : "N/A";
                        string subject = message.Subject ?? "N/A";
                        DateTime receivedDate = message.Date;

                        Console.WriteLine($"From: {from}");
                        Console.WriteLine($"Subject: {subject}");
                        Console.WriteLine($"Received: {receivedDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
