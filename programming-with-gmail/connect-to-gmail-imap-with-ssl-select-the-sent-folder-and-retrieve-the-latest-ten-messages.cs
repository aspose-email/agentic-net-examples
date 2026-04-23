using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // IMAP server settings (replace with real credentials)
            string host = "imap.gmail.com";
            int port = 993;
            string username = "your_username";
            string password = "your_password";

            // Guard against placeholder credentials
            if (username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Validate the credentials
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate credentials: {ex.Message}");
                    return;
                }

                // Select the Sent folder
                client.SelectFolder("Sent");

                // Retrieve the latest ten messages from the selected folder
                ImapMessageInfoCollection messageInfos = client.ListMessages(10);
                foreach (ImapMessageInfo info in messageInfos)
                {
                    using (MailMessage message = client.FetchMessage(info.UniqueId))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
