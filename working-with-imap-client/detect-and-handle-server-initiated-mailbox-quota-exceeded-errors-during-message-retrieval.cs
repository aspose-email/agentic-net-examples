using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are used
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Skipping network call due to placeholder credentials.");
                    return;
                }

                // Ensure output directory exists before any file operations
                string outputDirectory = "Output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Connect to the IMAP server and retrieve messages
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve a limited number of message infos
                        ImapMessageInfoCollection messageInfos = client.ListMessages(10);

                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            try
                            {
                                // Fetch the full message
                                MailMessage message = client.FetchMessage(messageInfo.UniqueId);

                                // Save the message to a file
                                string filePath = Path.Combine(outputDirectory, messageInfo.UniqueId + ".eml");
                                message.Save(filePath);
                            }
                            catch (ImapException imapEx)
                            {
                                // Detect quota exceeded condition
                                if (imapEx.Message != null && imapEx.Message.IndexOf("quota", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    Console.Error.WriteLine("Mailbox quota exceeded while retrieving message UID " + messageInfo.UniqueId + ": " + imapEx.Message);
                                }
                                else
                                {
                                    Console.Error.WriteLine("Error retrieving message UID " + messageInfo.UniqueId + ": " + imapEx.Message);
                                }
                            }
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        // Handle connection‑level IMAP errors
                        Console.Error.WriteLine("IMAP connection error: " + imapEx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
