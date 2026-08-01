using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

// Author: Aspose.Email example - encrypt and save a message as MSG
class Program
{
    static void Main()
    {
        try
        {
            // Path to the public certificate used for S/MIME encryption
            string certPath = "publicCert.cer";
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            X509Certificate2 publicCert = new X509Certificate2(certPath);

            // Create a simple email message
            MailMessage mail = new MailMessage();
            mail.From = "sender@example.com";
            mail.To.Add("recipient@example.com");
            mail.Subject = "Encrypted Message";
            mail.Body = "This is a confidential email.";

            // Encrypt the message using the certificate
            MailMessage encryptedMail = mail.Encrypt(publicCert);
            Console.WriteLine(encryptedMail.IsEncrypted ? "Message encrypted successfully." : "Encryption failed.");

            // Ensure the output directory exists before saving
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the encrypted message as MSG
            string outputPath = Path.Combine(outputDir, "encrypted.msg");
            encryptedMail.Save(outputPath);
            Console.WriteLine($"Encrypted MSG saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
