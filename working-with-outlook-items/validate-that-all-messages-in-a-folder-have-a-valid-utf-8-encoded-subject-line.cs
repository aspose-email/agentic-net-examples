using Aspose.Email.Clients;
using System;
using System.Text;
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
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";

            // Skip execution when using placeholder credentials/host
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client (constructor establishes the connection)
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP authentication failed: {ex.Message}");
                    return;
                }

                // Select the target folder
                try
                {
                    client.SelectFolder(folderName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder '{folderName}': {ex.Message}");
                    return;
                }

                // Retrieve message list from the selected folder
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

                // Validate UTF‑8 encoding of each message's subject
                foreach (ImapMessageInfo info in messages)
                {
                    MailMessage message;
                    try
                    {
                        message = client.FetchMessage(info.UniqueId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message UID {info.UniqueId}: {ex.Message}");
                        continue;
                    }

                    string subject = message.Subject ?? string.Empty;
                    byte[] utf8Bytes = Encoding.UTF8.GetBytes(subject);
                    string roundTrip = Encoding.UTF8.GetString(utf8Bytes);

                    if (subject != roundTrip)
                    {
                        Console.WriteLine($"Message UID {info.UniqueId} has an invalid UTF‑8 subject.");
                    }
                    else
                    {
                        Console.WriteLine($"Message UID {info.UniqueId} subject is valid UTF‑8.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
