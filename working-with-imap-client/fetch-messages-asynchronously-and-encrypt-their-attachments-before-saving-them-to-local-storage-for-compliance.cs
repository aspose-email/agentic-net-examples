using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // IMAP server configuration (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folder = "INBOX";
            string outputDirectory = "EncryptedAttachments";

            // Guard against placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
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

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the target folder
                    await client.SelectFolderAsync(folder);

                    // Retrieve message info collection
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(folder);

                    // Extract unique IDs for fetching
                    IEnumerable<string> uniqueIds = messageInfos.Select(info => info.UniqueId);

                    // Fetch messages asynchronously
                    IList<MailMessage> messages = await client.FetchMessagesAsync(uniqueIds);

                    // Process each message
                    foreach (MailMessage message in messages)
                    {
                        // Process each attachment
                        foreach (Attachment attachment in message.Attachments)
                        {
                            // Read attachment content into memory
                            using (MemoryStream originalStream = new MemoryStream())
                            {
                                attachment.ContentStream.CopyTo(originalStream);
                                byte[] encryptedData = Encrypt(originalStream.ToArray());

                                // Determine output path
                                string encryptedPath = Path.Combine(outputDirectory, attachment.Name ?? "attachment.dat");

                                // Write encrypted data to file
                                try
                                {
                                    File.WriteAllBytes(encryptedPath, encryptedData);
                                }
                                catch (Exception fileEx)
                                {
                                    Console.Error.WriteLine($"Failed to write encrypted attachment '{attachment.Name}': {fileEx.Message}");
                                }
                            }
                        }
                    }
                }
                catch (Exception imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple AES encryption (for demonstration purposes only)
    private static byte[] Encrypt(byte[] plainData)
    {
        // NOTE: In production, use a secure key/IV management strategy.
        byte[] key = new byte[32]; // 256‑bit zero key (placeholder)
        byte[] iv = new byte[16];  // 128‑bit zero IV (placeholder)

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            using (MemoryStream encryptedStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainData, 0, plainData.Length);
                cryptoStream.FlushFinalBlock();
                return encryptedStream.ToArray();
            }
        }
    }
}
