using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the encrypted EML input and the clear‑text MSG output
            string inputPath = "encrypted.eml";
            string outputPath = "decrypted.msg";

            // Verify that the input file exists before attempting to load it
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

            // Load the encrypted message from the EML file
            using (MailMessage encryptedMessage = MailMessage.Load(inputPath))
            {
                // Decrypt the message (uses certificate store; no explicit password needed)
                MailMessage decryptedMessage = encryptedMessage.Decrypt();

                // Ensure the decrypted message is disposed after saving
                using (decryptedMessage)
                {
                    // Save the clear‑text message as MSG
                    decryptedMessage.Save(outputPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Decrypted message saved to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            // Report any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
