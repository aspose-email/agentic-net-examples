using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve the list of messages in INBOX
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync("INBOX");

                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full MIME message for the current item
                        MailMessage mail = await client.FetchMessageAsync(info.UniqueId);
                        using (mail)
                        {
                            // Output basic MIME information
                            Console.WriteLine($"Subject: {mail.Subject}");
                            Console.WriteLine($"From: {mail.From}");
                            Console.WriteLine($"Date: {mail.Date}");

                            // Identify nested parts (attachments, alternate views, etc.)
                            if (mail.Attachments != null && mail.Attachments.Count > 0)
                            {
                                Console.WriteLine("Attachments:");
                                foreach (Attachment att in mail.Attachments)
                                {
                                    Console.WriteLine($" - {att.Name} ({att.ContentType.MediaType})");
                                }
                            }

                            if (mail.AlternateViews != null && mail.AlternateViews.Count > 0)
                            {
                                Console.WriteLine("Alternate Views:");
                                foreach (AlternateView view in mail.AlternateViews)
                                {
                                    Console.WriteLine($" - {view.ContentType.MediaType}");
                                }
                            }

                            Console.WriteLine(new string('-', 40));
                        }
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
        }
    }
}
