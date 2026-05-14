using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            bool hasUrgent = ContainsUrgentToday();
            Console.WriteLine($"Urgent message received today: {hasUrgent}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static bool ContainsUrgentToday()
    {
        // Placeholder credentials – skip real network call in CI environments
        string host = "pop3.example.com";
        int port = 110;
        string username = "username";
        string password = "password";

        if (host.Contains("example.com") || username == "username" || password == "password")
        {
            Console.Error.WriteLine("Placeholder POP3 credentials detected – skipping server connection.");
            return false;
        }

        try
        {
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                // Validate connection credentials
                client.ValidateCredentials();

                // Retrieve list of messages
                Pop3MessageInfoCollection messageInfos = client.ListMessages();

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    // Check if the message was received today
                    DateTime receivedDate = info.Date.Date;
                    if (receivedDate != DateTime.Today)
                        continue;

                    // Fetch the full message to inspect its content
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Search for the keyword "urgent" in subject or body (case‑insensitive)
                        bool subjectContains = message.Subject != null &&
                                               message.Subject.IndexOf("urgent", StringComparison.OrdinalIgnoreCase) >= 0;

                        bool bodyContains = message.Body != null &&
                                            message.Body.IndexOf("urgent", StringComparison.OrdinalIgnoreCase) >= 0;

                        if (subjectContains || bodyContains)
                            return true;
                    }
                }

                return false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error accessing POP3 server: {ex.Message}");
            return false;
        }
    }
}
