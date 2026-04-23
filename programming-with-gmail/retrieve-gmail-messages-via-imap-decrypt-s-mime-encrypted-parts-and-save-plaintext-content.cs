using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution in CI environments
            string host = "imap.gmail.com";
            string username = "your.email@gmail.com";
            string password = "yourpassword";

            if (host.Contains("imap.gmail.com") &&
                (username == "your.email@gmail.com" || password == "yourpassword"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputDir = "output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Connect to Gmail IMAP server
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve list of messages
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            MailMessage processedMessage = message;

                            // Decrypt if the message is encrypted
                            if (message.IsEncrypted)
                            {
                                try
                                {
                                    processedMessage = message.Decrypt();
                                }
                                catch (Exception decryptEx)
                                {
                                    Console.Error.WriteLine($"Failed to decrypt message UID {info.UniqueId}: {decryptEx.Message}");
                                    continue;
                                }
                            }

                            // Save plaintext body to a file
                            string filePath = Path.Combine(outputDir, $"{info.UniqueId}.txt");
                            try
                            {
                                File.WriteAllText(filePath, processedMessage.Body ?? string.Empty);
                            }
                            catch (Exception writeEx)
                            {
                                Console.Error.WriteLine($"Failed to write message UID {info.UniqueId} to file: {writeEx.Message}");
                            }

                            // Dispose decrypted message if it was a new instance
                            if (!ReferenceEquals(processedMessage, message))
                            {
                                processedMessage.Dispose();
                            }
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
