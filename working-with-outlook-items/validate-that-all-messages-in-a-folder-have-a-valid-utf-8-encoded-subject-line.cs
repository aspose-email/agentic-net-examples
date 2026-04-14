using System;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Connect to the IMAP server.
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the target folder (e.g., INBOX).
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder.
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Fetch the full message.
                        MailMessage mail = client.FetchMessage(info.UniqueId);

                        string subject = mail.Subject ?? string.Empty;

                        // Validate that the subject can be encoded and decoded as UTF‑8.
                        try
                        {
                            byte[] utf8Bytes = Encoding.UTF8.GetBytes(subject);
                            string roundTrip = Encoding.UTF8.GetString(utf8Bytes);

                            if (!subject.Equals(roundTrip, StringComparison.Ordinal))
                            {
                                Console.Error.WriteLine($"Message UID {info.UniqueId}: Subject contains characters that change after UTF‑8 round‑trip.");
                            }
                            else
                            {
                                Console.WriteLine($"Message UID {info.UniqueId}: Subject is valid UTF‑8.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Message UID {info.UniqueId}: Subject validation failed – {ex.Message}");
                        }
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
