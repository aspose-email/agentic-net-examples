using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the attachment and the output email file
            string attachmentPath = "sample.txt";
            string outputEmailPath = "encryptedEmail.eml";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputEmailPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            MailMessage message = new MailMessage(
                new MailAddress("sender@example.com"),
                new MailAddress("receiver@example.com"))
            {
                Subject = "Encrypted Attachment Example",
                Body = "Please find the encrypted attachment."
            };

            // Process the attachment: read, encrypt, and attach
            try
            {
                using (FileStream originalStream = File.OpenRead(attachmentPath))
                {
                    byte[] originalBytes = new byte[originalStream.Length];
                    int bytesRead = originalStream.Read(originalBytes, 0, originalBytes.Length);
                    if (bytesRead != originalBytes.Length)
                    {
                        Console.Error.WriteLine("Failed to read the entire attachment file.");
                        return;
                    }

                    byte[] encryptedBytes = EncryptBytes(originalBytes);

                    using (MemoryStream encryptedStream = new MemoryStream(encryptedBytes))
                    {
                        Attachment encryptedAttachment = new Attachment(encryptedStream, Path.GetFileName(attachmentPath));
                        message.Attachments.Add(encryptedAttachment);
                        // Note: the attachment does not need explicit disposal; it will be disposed with the message
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing attachment: {ex.Message}");
                return;
            }

            // Save the email message to a file
            try
            {
                using (FileStream outputStream = File.Create(outputEmailPath))
                {
                    message.Save(outputStream);
                }
                Console.WriteLine($"Email saved to '{outputEmailPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save email: {ex.Message}");
            }
            finally
            {
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple custom encryption: XOR each byte with 0xAA
    private static byte[] EncryptBytes(byte[] data)
    {
        byte[] encrypted = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            encrypted[i] = (byte)(data[i] ^ 0xAA);
        }
        return encrypted;
    }
}
