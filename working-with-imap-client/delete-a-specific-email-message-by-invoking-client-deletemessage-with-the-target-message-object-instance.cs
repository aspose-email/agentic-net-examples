using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values for actual execution
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";

        // Guard: skip network calls when placeholders are detected
        bool placeholders = host.Contains("example.com") ||
                            username.Contains("example.com") ||
                            password == "password";

        if (placeholders)
        {
            Console.WriteLine("Skipping network operation due to placeholder credentials.");
            return;
        }

        try
        {
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Placeholder UID of the message to delete
                string targetMessageUid = "12345";

                // Locate the message info with the specified UID
                ImapMessageInfoCollection messages = client.ListMessages();
                ImapMessageInfo targetInfo = null;
                foreach (var info in messages)
                {
                    if (info.UniqueId == targetMessageUid)
                    {
                        targetInfo = info;
                        break;
                    }
                }

                if (targetInfo == null)
                {
                    Console.WriteLine($"Message with UID {targetMessageUid} not found.");
                    return;
                }

                // Delete the message using its sequence number (or UID if supported)
                client.DeleteMessage(targetInfo.SequenceNumber);

                // If the ImapClient version supports Expunge, uncomment the following line:
                // client.Expunge();

                Console.WriteLine("Message deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
