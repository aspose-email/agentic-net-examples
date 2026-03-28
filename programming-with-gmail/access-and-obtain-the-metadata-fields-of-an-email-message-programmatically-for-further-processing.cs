using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize and configure the IMAP client
            using (ImapClient client = new ImapClient())
            {
                client.Host = "imap.example.com";
                client.Port = 993;
                client.SecurityOptions = SecurityOptions.SSLImplicit;
                client.Username = "user@example.com";
                client.Password = "password";

                // Connect to the server

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve basic information for all messages in the folder
                List<ImapMessageInfo> messages = client.ListMessages();

                foreach (ImapMessageInfo info in messages)
                {
                    // Fetch the full message using its unique identifier
                    using (MailMessage message = client.FetchMessage(info.UniqueId))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine($"Size: {info.Size} bytes");
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
