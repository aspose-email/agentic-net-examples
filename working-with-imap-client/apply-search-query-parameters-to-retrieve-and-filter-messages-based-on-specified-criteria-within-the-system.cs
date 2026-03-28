using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSearchExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize and connect the IMAP client
                using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the folder to search in
                        client.SelectFolder("INBOX");

                        // Build the search query
                        MailQueryBuilder builder = new MailQueryBuilder();
                        builder.Subject.Contains("Report");
                        builder.From.Contains("sender@example.com");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages matching the query
                        ImapMessageInfoCollection messages = client.ListMessages(query);

                        // Process each message
                        foreach (ImapMessageInfo info in messages)
                        {
                            MailMessage message = client.FetchMessage(info.UniqueId);
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {message.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
