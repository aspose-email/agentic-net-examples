using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string encryptedFilePath = "encryptedContact.eml";
            string certificatePath = "cert.pfx";
            string certificatePassword = "password";

            // Verify that the encrypted file exists
            if (!File.Exists(encryptedFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(encryptedFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Encrypted file not found: {encryptedFilePath}");
                return;
            }

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            try
            {
                // Load the certificate with the provided password
                X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

                // Load the encrypted message (contact) from file
                using (MailMessage encryptedMessage = MailMessage.Load(encryptedFilePath))
                {
                    if (!encryptedMessage.IsEncrypted)
                    {
                        Console.Error.WriteLine("The loaded message is not encrypted.");
                        return;
                    }

                    // Decrypt the message using the certificate
                    using (MailMessage decryptedMessage = encryptedMessage.Decrypt(certificate))
                    {
                        // Simple integrity check: ensure body is not empty
                        if (string.IsNullOrEmpty(decryptedMessage.Body))
                        {
                            Console.Error.WriteLine("Decryption succeeded but the message body is empty. Integrity check failed.");
                            return;
                        }

                        Console.WriteLine("Decryption successful. Message body length: " + decryptedMessage.Body.Length);

                        // Save the decrypted contact to a new file
                        string decryptedFilePath = "decryptedContact.eml";
                        decryptedMessage.Save(decryptedFilePath);
                        Console.WriteLine($"Decrypted contact saved to: {decryptedFilePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during decryption: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
