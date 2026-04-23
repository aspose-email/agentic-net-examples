using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailGmailSearch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string host = "imap.gmail.com";
                int port = 993;
                string username = "your.email@gmail.com";
                string password = "yourpassword";

                // Guard against running with placeholder data.
                if (host.Contains("example") || username.Contains("example") || password.Contains("example"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                    return;
                }

                // Create and connect the IMAP client.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Select the INBOX folder.
                        client.SelectFolder("INBOX");

                        // Build a complex query: messages sent between two dates and containing a keyword in the body.
                        MailQuery query = new MailQuery(
                            "SentDate >= '01-Jan-2023' AND SentDate <= '31-Jan-2023' AND Body Contains 'Aspose'");

                        // Retrieve matching messages.
                        ImapMessageInfoCollection messages = client.ListMessages(query);

                        // Output basic information for each matched message.
                        foreach (ImapMessageInfo info in messages)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                            Console.WriteLine($"Sent: {info.Date}");
                            Console.WriteLine($"From: {info.From}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
