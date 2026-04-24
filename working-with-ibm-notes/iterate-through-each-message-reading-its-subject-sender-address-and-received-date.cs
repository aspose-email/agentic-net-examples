using Aspose.Email.Clients;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;
class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials or host
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping connection.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Validate credentials safely
                client.ValidateCredentials();

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve message information collection
                ImapMessageInfoCollection messageInfos = client.ListMessages();

                // Iterate through each message and output details
                foreach (ImapMessageInfo messageInfo in messageInfos)
                {
                    Console.WriteLine("Subject: " + messageInfo.Subject);
                    Console.WriteLine("From: " + (messageInfo.From != null ? messageInfo.From.ToString() : "N/A"));
                    Console.WriteLine("Received: " + messageInfo.Date);
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
