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
            string emlPath = "message.eml";

            // Guard against missing input file
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

                Console.Error.WriteLine($"Input file '{emlPath}' does not exist.");
                return;
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                if (mailMessage.Attachments.Count == 0)
                {
                    Console.WriteLine("No attachments found.");
                    return;
                }

                // Iterate through each attachment and compute its MD5 hash
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Copy attachment content to a memory stream
                        attachment.ContentStream.CopyTo(memoryStream);
                        byte[] attachmentBytes = memoryStream.ToArray();

                        // Compute MD5 hash
                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] hashBytes = md5.ComputeHash(attachmentBytes);
                            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                            Console.WriteLine($"Attachment: {attachment.Name}, MD5: {hashString}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
