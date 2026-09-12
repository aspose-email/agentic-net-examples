using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSearchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Author note: This example demonstrates how to apply a MailQuery to filter IMAP messages.
            try
            {
                // IMAP server connection parameters (replace with real values).
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

                // Create the IMAP client with automatic SSL security.
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Define a search query: unread messages with "Invoice" in the subject.
                    MailQuery query = new MailQuery("(('Subject' Contains 'Invoice') & 'Seen' = 'False')");

                    // Retrieve messages that match the query from the currently selected folder (INBOX by default).
                    ImapMessageInfoCollection messages = imapClient.ListMessages(query);

                    // Output basic information about each matched message.
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                        Console.WriteLine($"Date: {messageInfo.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Gracefully exit without rethrowing.
            }
        }
    }
}
