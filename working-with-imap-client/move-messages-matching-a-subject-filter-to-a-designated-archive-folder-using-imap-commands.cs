using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";
            string archiveFolder = "Archive";
            string subjectFilter = "YourSubjectFilter";

            // Guard against placeholder credentials
            if (host.Contains("example.com") || username.Contains("username"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;

                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Ensure the archive folder exists
                    if (!client.ExistFolder(archiveFolder))
                    {
                        client.CreateFolder(archiveFolder);
                    }

                    // Retrieve all messages in INBOX
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    List<string> uidsToMove = new List<string>();
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message to examine the subject
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        if (message.Subject != null && message.Subject.Contains(subjectFilter))
                        {
                            uidsToMove.Add(info.UniqueId);
                        }
                    }

                    if (uidsToMove.Count > 0)
                    {
                        // Move matching messages to the archive folder
                        client.MoveMessagesAsync(uidsToMove, archiveFolder, CancellationToken.None).Wait();
                        Console.WriteLine($"{uidsToMove.Count} message(s) moved to '{archiveFolder}'.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the subject filter.");
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
