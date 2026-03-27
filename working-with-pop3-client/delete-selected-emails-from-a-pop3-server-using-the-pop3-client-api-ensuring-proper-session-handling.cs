using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace Pop3DeleteSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection settings
                string host = "pop.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client inside a using block for proper disposal
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Build a query to select messages whose subject contains "Spam"
                        MailQueryBuilder builder = new MailQueryBuilder();
                        builder.Subject.Contains("Spam");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages matching the query
                        var messageInfos = client.ListMessages(query);
                        foreach (var info in messageInfos)
                        {
                            // Mark each selected message for deletion
                            client.DeleteMessage(info.SequenceNumber);
                        }

                        // Commit deletions so the server permanently removes the marked messages
                        client.CommitDeletes();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
}
