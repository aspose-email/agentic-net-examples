using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace ImapQuerySample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define connection parameters (replace with real values)
            string host = "your_imap_host";
            int port = 993;
            string username = "your_username";
            string password = "your_password";

            // Guard against placeholder credentials
            if (host.Contains("your_") || username.Contains("your_") || password.Contains("your_"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping IMAP operation.");
                return;
            }

            try
            {
                // Initialize and configure the IMAP client
                ImapClient client = new ImapClient();
                client.Host = host;
                client.Port = port;
                client.SecurityOptions = SecurityOptions.Auto;
                client.Username = username;
                client.Password = password;

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Build a simple search query (e.g., messages with "Invoice" in the subject)
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.Subject.Contains("Invoice");
                MailQuery query = queryBuilder.GetQuery();

                // Retrieve messages matching the query
                ImapMessageInfoCollection messages = client.ListMessages(query);

                // Output basic information about each matched message
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}, From: {info.From}");
                }

                // Dispose the client
                client.Dispose();
            }
            catch (Exception ex)
            {
                // Gracefully handle any errors (network, authentication, etc.)
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
