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
            // POP3 server connection parameters (placeholders)
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 server detected. Skipping operation.");
                return;
            }

            // Pattern to match in the subject
            string subjectPattern = "Invoice";

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Build a query to retrieve messages (optional, can retrieve all)
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains(subjectPattern);
                    MailQuery query = builder.GetQuery();

                    // List messages matching the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // Delete messages whose subject contains the pattern
                    foreach (Pop3MessageInfo info in messages)
                    {
                        if (!string.IsNullOrEmpty(info.Subject) && info.Subject.Contains(subjectPattern))
                        {
                            client.DeleteMessage(info.SequenceNumber);
                        }
                    }

                    // Commit deletions on the server
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    // No rethrow; ensure client is disposed by using block
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
