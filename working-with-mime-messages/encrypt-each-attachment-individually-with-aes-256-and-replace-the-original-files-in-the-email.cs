using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            string outputPath = "output.eml";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Copy current attachments to a list to avoid modifying the collection while iterating
                    List<Attachment> originalAttachments = new List<Attachment>(message.Attachments);

                    foreach (Attachment originalAttachment in originalAttachments)
                    {
                        // Read original attachment data
                        using (MemoryStream originalStream = new MemoryStream())
                        {
                            originalAttachment.ContentStream.CopyTo(originalStream);
                            byte[] originalBytes = originalStream.ToArray();

                            // Encrypt the attachment data with AES‑256
                            byte[] encryptedBytes = EncryptWithAes256(originalBytes);

                            // Create a new attachment from the encrypted data
                            MemoryStream encryptedStream = new MemoryStream(encryptedBytes);
                            Attachment encryptedAttachment = new Attachment(encryptedStream, originalAttachment.Name);

                            // Replace the original attachment with the encrypted one
                            message.Attachments.Remove(originalAttachment);
                            message.Attachments.Add(encryptedAttachment);
                        }
                    }

                    // Save the modified message
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with encrypted attachments to '{outputPath}'.");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File operation failed: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static byte[] EncryptWithAes256(byte[] plainData)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // NOTE: For demonstration purposes only. In production use a securely generated key and IV.
            aes.Key = new byte[32]; // 256‑bit zero key
            aes.IV = new byte[16];  // 128‑bit zero IV

            using (MemoryStream encryptedStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainData, 0, plainData.Length);
                    cryptoStream.FlushFinalBlock();
                }
                return encryptedStream.ToArray();
            }
        }
    }
}
