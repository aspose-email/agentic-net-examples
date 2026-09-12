using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings for the IMAP server
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize and configure the IMAP client
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.SecurityOptions = SecurityOptions.SSLImplicit;
                client.Username = username;
                client.Password = password;

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Build a search query (e.g., messages with "Invoice" in the subject)
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.Subject.Contains("Invoice");
                MailQuery query = queryBuilder.GetQuery();

                // Execute the search
                ImapMessageInfoCollection matchingMessages = client.ListMessages(query);

                Console.WriteLine($"Found {matchingMessages.Count} message(s) matching the criteria.");

                foreach (ImapMessageInfo messageInfo in matchingMessages)
                {
                    Console.WriteLine($"UID: {messageInfo.UniqueId}, Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
