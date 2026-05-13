using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "message.eml";

            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(emlPath))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    // Compute SHA256 checksum of the attachment content
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        // Ensure the stream is at the beginning
                        if (attachment.ContentStream.CanSeek)
                        {
                            attachment.ContentStream.Position = 0;
                        }

                        byte[] hashBytes = sha256.ComputeHash(attachment.ContentStream);
                        string computedChecksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                        // Retrieve expected checksum from custom header "X-Checksum"
                        string expectedChecksum = null;
                        foreach (string key in attachment.Headers.Keys)
                        {
                            if (string.Equals(key, "X-Checksum", StringComparison.OrdinalIgnoreCase))
                            {
                                expectedChecksum = attachment.Headers[key];
                                break;
                            }
                        }

                        if (expectedChecksum != null)
                        {
                            if (string.Equals(computedChecksum, expectedChecksum, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"Attachment '{attachment.Name}' checksum matches.");
                            }
                            else
                            {
                                Console.WriteLine($"Attachment '{attachment.Name}' checksum mismatch! Expected {expectedChecksum}, got {computedChecksum}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Attachment '{attachment.Name}' does not contain a checksum header.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
