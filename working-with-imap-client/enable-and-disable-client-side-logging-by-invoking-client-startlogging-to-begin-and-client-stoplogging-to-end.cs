using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IMAP client with placeholder credentials
            using (ImapClient client = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
            {
                // Enable client-side logging
                client.EnableLogger = true;
                client.LogFileName = "imap_log.txt";

                // Connect to the server (connection is established lazily on first operation)
                // Perform a simple operation to verify connection
                client.SelectFolder("INBOX");

                // Example: list subjects of messages in INBOX
                MailQueryBuilder builder = new MailQueryBuilder();
                MailQuery query = builder.GetQuery(); // no criteria, fetch all
                var messages = client.ListMessages(query);
                foreach (var info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                }

                // Disable client-side logging
                client.EnableLogger = false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
