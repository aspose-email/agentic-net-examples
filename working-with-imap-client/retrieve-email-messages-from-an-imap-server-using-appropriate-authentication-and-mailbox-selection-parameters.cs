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
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Create the IMAP client
            ImapClient client = null;
            try
            {
                client = new ImapClient(host, port, username, password, security);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create IMAP client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly
            using (client)
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the selected folder
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    // Iterate through each message info and fetch the full message
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP processing: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
