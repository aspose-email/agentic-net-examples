using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server connection details (replace with real values)
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Subject pattern to match for deletion
                string subjectPattern = "Invoice";

                // Initialize POP3 client inside a using block to ensure disposal
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    // Build a query (required by POP3 rule) – fetch all messages
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains(subjectPattern);
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // Iterate through messages and delete those whose subject contains the pattern
                    foreach (Pop3MessageInfo info in messages)
                    {
                        if (info.Subject != null && info.Subject.Contains(subjectPattern))
                        {
                            client.DeleteMessage(info.SequenceNumber);
                        }
                    }

                    // Commit deletions to finalize removal on the server
                    client.CommitDeletes();
                }
            }
            catch (Exception ex)
            {
                // Friendly error output without crashing the application
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
