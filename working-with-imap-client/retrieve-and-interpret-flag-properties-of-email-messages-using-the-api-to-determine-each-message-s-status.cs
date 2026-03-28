using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace EmailFlagStatusExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // IMAP server connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Connect to the IMAP server and process messages
                try
                {
                    using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                    {
                        // Retrieve the list of messages in the default folder (INBOX)
                        ImapMessageInfoCollection messages = client.ListMessages();

                        foreach (ImapMessageInfo info in messages)
                        {
                            // Build a readable flag description
                            string flagDescription = string.Empty;
                            if (info.Answered) flagDescription += "Answered ";
                            if (info.Deleted) flagDescription += "Deleted ";
                            if (info.Draft) flagDescription += "Draft ";
                            if (info.Flagged) flagDescription += "Flagged ";
                            if (info.IsRead) flagDescription += "Read ";
                            if (info.Recent) flagDescription += "Recent ";

                            // Trim trailing space
                            flagDescription = flagDescription.Trim();

                            // Output message UID, subject and flags
                            Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}, Flags: {flagDescription}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
