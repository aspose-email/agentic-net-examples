using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";
            string folder = "INBOX";
            int sequenceNumber = 1;

            // Guard against placeholder credentials/host.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details provided. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.SelectFolder(folder);

                    // Set the Read flag on the message.
                    client.ChangeMessageFlags(sequenceNumber, ImapMessageFlags.IsRead);

                    // Fetch the message to display its subject.
                    using (MailMessage message = client.FetchMessage(sequenceNumber))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }

                    // Retrieve message info to verify the flag persisted.
                    using (ImapMessageInfo info = client.ListMessage(sequenceNumber))
                    {
                        Console.WriteLine($"IsRead flag after change: {info.IsRead}");
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
