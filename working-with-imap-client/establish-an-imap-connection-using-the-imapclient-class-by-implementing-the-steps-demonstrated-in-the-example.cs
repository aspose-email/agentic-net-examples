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
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Validate credentials
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine($"Authentication failed: {credEx.Message}");
                    return;
                }

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // List messages in the selected folder
                ImapMessageInfoCollection messageInfos = client.ListMessages();
                Console.WriteLine($"Total messages: {messageInfos.Count}");

                // Fetch and display the first message, if any
                if (messageInfos.Count > 0)
                {
                    ImapMessageInfo firstInfo = messageInfos[0];
                    MailMessage firstMessage = client.FetchMessage(firstInfo.UniqueId);
                    Console.WriteLine($"Subject: {firstMessage.Subject}");
                    firstMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
