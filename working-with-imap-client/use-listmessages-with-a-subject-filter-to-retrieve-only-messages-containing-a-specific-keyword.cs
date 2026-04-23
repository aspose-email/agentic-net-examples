using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping IMAP operations.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;

                    // Select the INBOX folder (acts as a lightweight validation)
                    client.SelectFolder("INBOX");

                    // Build a query to filter messages whose subject contains a keyword
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    queryBuilder.Subject.Contains("keyword");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve messages matching the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Output the subjects of the matching messages
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("IMAP operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
