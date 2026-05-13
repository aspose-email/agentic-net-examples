using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the encrypted email file (EML)
            string encryptedEmlPath = "encrypted.eml";
            // Path where the decrypted attachment will be saved
            string decryptedAttachmentPath = "decryptedAttachment.dat";

            // Verify that the input file exists
            if (!File.Exists(encryptedEmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(encryptedEmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {encryptedEmlPath}");
                return;
            }

            // Load the encrypted message
            using (MailMessage encryptedMessage = MailMessage.Load(encryptedEmlPath))
            {
                // Decrypt the message (handles PGP/S‑MIME encrypted content)
                MailMessage decryptedMessage = encryptedMessage.Decrypt();

                // Ensure there is at least one attachment
                if (decryptedMessage.Attachments.Count == 0)
                {
                    Console.Error.WriteLine("No attachments found in the decrypted message.");
                    return;
                }

                // Get the first attachment
                Aspose.Email.Attachment attachment = decryptedMessage.Attachments[0];

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(decryptedAttachmentPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the attachment to the specified path
                attachment.Save(decryptedAttachmentPath);
                Console.WriteLine($"Attachment saved to: {decryptedAttachmentPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
