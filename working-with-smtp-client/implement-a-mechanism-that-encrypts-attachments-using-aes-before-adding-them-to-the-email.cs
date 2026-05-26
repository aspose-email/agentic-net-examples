using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input file to be attached
            string inputFilePath = "sample.txt";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputFilePath))
            {
                try
                {
                    File.WriteAllText(inputFilePath, "This is a sample attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Read the file bytes
            byte[] fileBytes;
            try
            {
                fileBytes = File.ReadAllBytes(inputFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read input file: {ex.Message}");
                return;
            }

            // AES encryption setup (demo key/IV; in real scenarios store securely)
            byte[] encryptedBytes;
            using (Aes aes = Aes.Create())
            {
                aes.Key = new byte[32]; // 256‑bit zero key (for demo only)
                aes.IV = new byte[16];  // 128‑bit zero IV (for demo only)

                using (MemoryStream output = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedBytes = output.ToArray();
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with AES‑encrypted attachment";
                message.Body = "Please find the encrypted attachment.";

                // Add the encrypted attachment from memory
                using (MemoryStream encryptedStream = new MemoryStream(encryptedBytes))
                {
                    // The attachment name can indicate it is encrypted
                    Attachment encryptedAttachment = new Attachment(encryptedStream, Path.GetFileName(inputFilePath) + ".enc");
                    message.Attachments.Add(encryptedAttachment);
                }

                // Output file path
                string outputFilePath = "encrypted_email.eml";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Save the message to an EML file
                try
                {
                    message.Save(outputFilePath);
                    Console.WriteLine($"Email saved to {outputFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
