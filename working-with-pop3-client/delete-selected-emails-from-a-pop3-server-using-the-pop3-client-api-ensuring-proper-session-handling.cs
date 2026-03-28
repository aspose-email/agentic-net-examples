using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection parameters (replace with real values)
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Create and use the POP3 client inside a using block to ensure disposal
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Build a query to select messages that contain "Sample" in the subject
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Sample");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages that match the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // Delete each selected message by its sequence number
                    foreach (Pop3MessageInfo info in messages)
                    {
                        client.DeleteMessage(info.SequenceNumber);
                    }

                    // Commit deletions so the server removes the marked messages
                    client.CommitDeletes();
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during POP3 operations
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors in the application
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
