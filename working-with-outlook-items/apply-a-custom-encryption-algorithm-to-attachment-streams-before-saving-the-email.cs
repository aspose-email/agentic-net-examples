using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the attachment and the output email
            string attachmentPath = "sample.txt";
            string emailOutputPath = "encrypted_email.eml";

            
            string outputDir = Path.GetDirectoryName(emailOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
// Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder content for attachment.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Read the original attachment bytes
            byte[] originalBytes;
            try
            {
                originalBytes = File.ReadAllBytes(attachmentPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read attachment file: {ex.Message}");
                return;
            }

            // Apply a simple XOR encryption to the bytes
            byte encryptionKey = 0xAA;
            byte[] encryptedBytes = new byte[originalBytes.Length];
            for (int i = 0; i < originalBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(originalBytes[i] ^ encryptionKey);
            }

            // Create a memory stream from the encrypted bytes
            using (MemoryStream encryptedStream = new MemoryStream(encryptedBytes))
            {
                // Create the attachment with the encrypted stream
                Attachment encryptedAttachment = new Attachment(encryptedStream, "application/octet-stream");
                encryptedAttachment.Name = "encrypted_sample.txt";

                // Build the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To = "receiver@example.com";
                    message.Subject = "Email with Encrypted Attachment";
                    message.Body = "Please find the encrypted attachment attached.";

                    // Add the encrypted attachment
                    message.Attachments.Add(encryptedAttachment);

                    // Save the email to a file
                    try
                    {
                        message.Save(emailOutputPath);
                        Console.WriteLine($"Email saved to '{emailOutputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save email: {ex.Message}");
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
