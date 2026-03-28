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
            // Initialize IMAP client with connection parameters
            using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                // Select the folder to search in
                client.SelectFolder("INBOX");

                // Build the search query (e.g., messages with "Report" in the subject)
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report");
                MailQuery query = builder.GetQuery();

                // Execute the search using ListMessages with the built query
                ImapMessageInfoCollection messages = client.ListMessages(query);

                // Process the resulting messages
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
