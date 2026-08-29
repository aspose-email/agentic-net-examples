using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace MailQueryExample
{
    class Program
    {
        static void Main()
        {
            // Configure IMAP client with placeholder values.
            ImapClient imapClient = new ImapClient
            {
                Host = "imap.example.com",
                Port = 993,
                Username = "user@example.com",
                Password = "password",
                SecurityOptions = SecurityOptions.Auto
            };

            // Guard: skip network operations when placeholders are detected.
            bool hasPlaceholder = imapClient.Host.Contains("example.com") ||
                                  imapClient.Username.Contains("example.com") ||
                                  imapClient.Password == "password";

            if (hasPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            try
            {
                using (imapClient)
                {
                    // Select the folder to search.
                    imapClient.SelectFolder("INBOX");

                    // Build a MailQuery that looks for a keyword in the subject.
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Important");
                    MailQuery subjectQuery = queryBuilder.GetQuery();

                    // Retrieve messages that match the query.
                    ImapMessageInfoCollection matchedMessages = imapClient.ListMessages(subjectQuery);

                    // Iterate over the matched messages and display their subjects.
                    foreach (ImapMessageInfo messageInfo in matchedMessages)
                    {
                        // Fetch the full message to access its Subject property.
                        MailMessage fullMessage = imapClient.FetchMessage(messageInfo.UniqueId);
                        Console.WriteLine($"Subject: {fullMessage.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
