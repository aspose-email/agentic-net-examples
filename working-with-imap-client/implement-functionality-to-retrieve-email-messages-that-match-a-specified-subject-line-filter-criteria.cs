using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace EmailSubjectFilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // IMAP server connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create and use the IMAP client
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(
                    host,
                    port,
                    username,
                    password,
                    Aspose.Email.Clients.SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        imapClient.SelectFolder("INBOX");

                        // Build a query to filter messages by subject
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Your Subject Filter");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages that match the query
                        Aspose.Email.Clients.Imap.ImapMessageInfoCollection messages = imapClient.ListMessages(query);

                        // Iterate through the matched messages
                        foreach (Aspose.Email.Clients.Imap.ImapMessageInfo info in messages)
                        {
                            // Fetch the full message
                            using (Aspose.Email.MailMessage message = imapClient.FetchMessage(info.UniqueId))
                            {
                                Console.WriteLine("Subject: " + (message.Subject ?? string.Empty));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("IMAP operation error: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}