using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates loading an encrypted EML, decrypting it, and saving as MSG.
            string inputPath = "encrypted.eml";
            string outputPath = "decrypted.msg";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the encrypted EML message
            using (MailMessage encryptedMessage = MailMessage.Load(inputPath))
            {
                // Decrypt the message (uses default decryption mechanism)
                using (MailMessage decryptedMessage = encryptedMessage.Decrypt())
                {
                    // Save the clear‑text message as MSG
                    decryptedMessage.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
