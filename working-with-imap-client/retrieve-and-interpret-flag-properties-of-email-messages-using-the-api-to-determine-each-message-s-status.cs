using Aspose.Email.Clients;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values when needed
        string host = "imap.example.com";
        int port = 993;
        SecurityOptions security = SecurityOptions.SSLImplicit;
        string username = "user@example.com";
        string password = "password";

        // Guard against placeholder credentials
        if (host.Contains("example.com") ||
            username.Contains("example.com") ||
            password == "password")
        {
            Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
            return;
        }

        try
        {
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.SecurityOptions = security;
                imapClient.Username = username;
                imapClient.Password = password;

                // Retrieve messages from the default folder (INBOX)
                ImapMessageInfoCollection messages = imapClient.ListMessages();

                if (messages != null && messages.Count > 0)
                {
                    foreach (ImapMessageInfo msgInfo in messages)
                    {
                        ImapMessageFlags flags = msgInfo.Flags;

                        Console.WriteLine($"Message UID: {msgInfo.UniqueId}");
                        Console.WriteLine($"  Answered: {flags.HasFlag(ImapMessageFlags.Answered)}");
                        Console.WriteLine($"  Deleted : {flags.HasFlag(ImapMessageFlags.Deleted)}");
                        Console.WriteLine($"  Draft   : {flags.HasFlag(ImapMessageFlags.Draft)}");
                        Console.WriteLine($"  Flagged : {flags.HasFlag(ImapMessageFlags.Flagged)}");
                        Console.WriteLine($"  IsRead  : {flags.HasFlag(ImapMessageFlags.IsRead)}");
                        Console.WriteLine($"  Recent  : {flags.HasFlag(ImapMessageFlags.Recent)}");
                    }
                }
                else
                {
                    Console.WriteLine("No messages found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
