using System;
using System.IO;
using Aspose.Email;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PFX file and its password
            string pfxPath = "certificate.pfx";
            string pfxPassword = "password";

            // Verify that the PFX file exists
            if (!File.Exists(pfxPath))
            {
                Console.Error.WriteLine($"PFX file not found: {pfxPath}");
                return;
            }

            // Load the certificate
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(pfxPath, pfxPassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Create and configure the email message
            using (certificate)
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Test Subject";
                message.Body = "Test Body";

                // Encrypt the message using the loaded certificate
                MailMessage encryptedMessage = message.Encrypt(certificate);

                // Prepare output path for the encrypted message
                string emlPath = "encrypted.eml";
                string emlDirectory = Path.GetDirectoryName(emlPath);
                if (!string.IsNullOrEmpty(emlDirectory) && !Directory.Exists(emlDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(emlDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory: {dirEx.Message}");
                        encryptedMessage.Dispose();
                        return;
                    }
                }

                // Save the encrypted message to a file
                try
                {
                    encryptedMessage.Save(emlPath);
                    Console.WriteLine($"Encrypted message saved to {emlPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save encrypted message: {saveEx.Message}");
                }

                // Clean up the encrypted message instance
                encryptedMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
