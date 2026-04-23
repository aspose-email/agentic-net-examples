using Aspose.Email.Mime;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution in CI.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";

            // Skip actual network call when placeholders are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP settings detected. Skipping connection.");
                return;
            }

            // Connect to the IMAP server and select the target folder.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                client.SelectFolder(folderName);

                // Retrieve a collection of message infos (headers only, no bodies).
                ImapMessageInfoCollection messageInfos = client.ListMessages();

                foreach (ImapMessageInfo info in messageInfos)
                {
                    Console.WriteLine($"Message UID: {info.UniqueId}");
                    HeaderCollection headers = info.Headers;

                    // Iterate over the header key/value pairs.
                    foreach (string header in headers.Keys)
                    {
                        Console.WriteLine($"{header}: {headers[header]}");
                    }

                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
