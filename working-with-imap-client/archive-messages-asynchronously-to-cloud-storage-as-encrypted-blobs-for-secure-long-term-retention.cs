using Aspose.Email.Clients;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    // Entry point wrapped in a top‑level exception guard.
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials.
            if (host.Contains("example.com") || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                return;
            }

            // Output directory for encrypted blobs.
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "EncryptedArchive");

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and use the IMAP client inside a using block.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Wrap client operations in a try/catch to surface friendly errors.
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder.
                    Task<ImapMessageInfoCollection> listTask = client.ListMessagesAsync();
                    ImapMessageInfoCollection messageInfos = await listTask.ConfigureAwait(false);

                    // Collect the unique identifiers of all messages.
                    List<string> uidList = new List<string>();
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        uidList.Add(info.UniqueId);
                    }

                    // Fetch all messages asynchronously.
                    Task<IList<MailMessage>> fetchTask = client.FetchMessagesAsync(uidList);
                    IList<MailMessage> messages = await fetchTask.ConfigureAwait(false);

                    // Encrypt each message and store it as a blob.
                    foreach (MailMessage mailMessage in messages)
                    {
                        // Derive a simple filename from the message subject (sanitized) and a timestamp.
                        string safeSubject = mailMessage.Subject ?? "NoSubject";
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }
                        string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{safeSubject}.enc";
                        string filePath = Path.Combine(outputDirectory, fileName);

                        // Convert the message to its raw MIME representation.
                        string mimeContent = mailMessage.ToString();
                        byte[] plainBytes = Encoding.UTF8.GetBytes(mimeContent);

                        // Encrypt the MIME content using AES.
                        try
                        {
                            using (Aes aes = Aes.Create())
                            {
                                // For demonstration purposes, a static key/IV is used.
                                // In production, use a securely generated key and store it safely.
                                aes.Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF"); // 32 bytes for AES‑256
                                aes.IV = Encoding.UTF8.GetBytes("ABCDEF0123456789"); // 16 bytes

                                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                }
                            }
                        }
                        catch (Exception encEx)
                        {
                            Console.Error.WriteLine($"Encryption failed for message '{mailMessage.Subject}': {encEx.Message}");
                            // Continue with next message.
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
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
