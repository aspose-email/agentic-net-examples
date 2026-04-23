using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            string msgPath = "sample.msg";
            string outputDirectory = "EncryptedAttachments";

            // Verify MSG file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Generate AES key and IV (for demonstration purposes)
                byte[] aesKey;
                byte[] aesIv;
                using (Aes aesGenerator = Aes.Create())
                {
                    aesGenerator.GenerateKey();
                    aesGenerator.GenerateIV();
                    aesKey = aesGenerator.Key;
                    aesIv = aesGenerator.IV;
                }

                Console.WriteLine($"AES Key (Base64): {Convert.ToBase64String(aesKey)}");
                Console.WriteLine($"AES IV  (Base64): {Convert.ToBase64String(aesIv)}");

                // Process each attachment
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    try
                    {
                        // Save attachment to a memory stream
                        using (MemoryStream attachmentStream = new MemoryStream())
                        {
                            attachment.Save(attachmentStream);
                            byte[] plainBytes = attachmentStream.ToArray();

                            // Encrypt the attachment bytes using AES
                            using (Aes aes = Aes.Create())
                            {
                                aes.Key = aesKey;
                                aes.IV = aesIv;

                                using (MemoryStream encryptedStream = new MemoryStream())
                                {
                                    using (CryptoStream crypto = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                    {
                                        crypto.Write(plainBytes, 0, plainBytes.Length);
                                        crypto.FlushFinalBlock();

                                        byte[] encryptedBytes = encryptedStream.ToArray();

                                        // Write encrypted bytes to disk
                                        string encryptedFilePath = Path.Combine(outputDirectory, attachment.FileName + ".enc");
                                        try
                                        {
                                            File.WriteAllBytes(encryptedFilePath, encryptedBytes);
                                            Console.WriteLine($"Encrypted attachment saved: {encryptedFilePath}");
                                        }
                                        catch (Exception ioEx)
                                        {
                                            Console.Error.WriteLine($"Failed to write encrypted file '{encryptedFilePath}': {ioEx.Message}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Error processing attachment '{attachment.FileName}': {attEx.Message}");
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
