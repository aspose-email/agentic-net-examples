using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace RetrieveMessagesById
{
    class Program
    {
        static void Main()
        {
            // Author: Aspose.Email example - fetch messages by unique identifiers (UIDs) using IMAP.
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

            // Create the IMAP client with SSL implicit security.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the folder to work with (e.g., INBOX).
                    client.SelectFolder("INBOX");

                    // List of message UIDs to retrieve.
                    List<string> messageUids = new List<string> { "1", "2", "3" };

                    // Fetch the messages corresponding to the supplied UIDs.
                    IList<MailMessage> messages = client.FetchMessages(messageUids);

                    // Process the retrieved messages.
                    foreach (MailMessage message in messages)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"An error occurred while fetching messages: {ex.Message}");
                }
            }
        }
    }
}
