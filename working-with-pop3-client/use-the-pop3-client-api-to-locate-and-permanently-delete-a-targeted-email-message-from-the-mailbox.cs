using Aspose.Email.Tools.Search;
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
            // Placeholder POP3 server credentials
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (host.Contains("example") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Build a query to locate the target message (e.g., by subject)
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Target Subject");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // Find the first message that matches the criteria
                    foreach (Pop3MessageInfo info in messages)
                    {
                        if (info.Subject != null && info.Subject.Contains("Target Subject"))
                        {
                            // Mark the message for deletion using its sequence number
                            client.DeleteMessage(info.SequenceNumber);

                            // Permanently delete all messages marked for deletion
                            Console.WriteLine($"Message with UID '{info.UniqueId}' has been permanently deleted.");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
