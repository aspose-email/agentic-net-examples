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
            // POP3 server credentials
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client inside a using block for proper disposal
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Build a query to locate the targeted email (e.g., subject contains "Target")
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Target");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // If a matching message is found, delete it permanently
                    if (messages != null && messages.Count > 0)
                    {
                        // Delete the first matching message by its sequence number
                        int sequenceNumber = messages[0].SequenceNumber;
                        client.DeleteMessage(sequenceNumber);

                        // Commit deletions to permanently remove the message from the mailbox
                        client.CommitDeletes();
                        Console.WriteLine("Message deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No matching messages found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operations: {ex.Message}");
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
