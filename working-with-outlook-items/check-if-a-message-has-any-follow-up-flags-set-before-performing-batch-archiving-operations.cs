using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient())
            {
                try
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Ensure the archive folder exists
                try
                {
                    if (!client.ExistFolder("Archive"))
                    {
                        client.CreateFolder("Archive");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to verify/create archive folder: {ex.Message}");
                    return;
                }

                // Select the source folder (INBOX)
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select INBOX: {ex.Message}");
                    return;
                }

                // Retrieve all messages in the folder
                ImapMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Identify messages with the follow‑up (Flagged) flag
                List<int> flaggedMessageIds = new List<int>();
                foreach (ImapMessageInfo messageInfo in messages)
                {
                    if (messageInfo.Flagged)
                    {
                        flaggedMessageIds.Add(messageInfo.SequenceNumber);
                    }
                }

                // If no flagged messages, nothing to archive
                if (flaggedMessageIds.Count == 0)
                {
                    Console.WriteLine("No messages with follow‑up flags found.");
                    return;
                }

                // Move flagged messages to the Archive folder
                try
                {
                    client.MoveMessages(flaggedMessageIds, "Archive");
                    Console.WriteLine($"Archived {flaggedMessageIds.Count} message(s) with follow‑up flags.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to move messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
