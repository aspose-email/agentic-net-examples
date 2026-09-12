using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to certificate files
            string publicCertPath = "RecipientCertificate.cer";
            string signingCertPath = "SigningCertificate.pfx";
            string signingCertPassword = "password";

            // Verify certificate files exist
            if (!File.Exists(publicCertPath))
            {
                Console.Error.WriteLine($"Public certificate file not found: {publicCertPath}");
                return;
            }

            if (!File.Exists(signingCertPath))
            {
                Console.Error.WriteLine($"Signing certificate file not found: {signingCertPath}");
                return;
            }

            // Load certificates
            X509Certificate2 publicCert = new X509Certificate2(publicCertPath);
            X509Certificate2 signingCert = new X509Certificate2(signingCertPath, signingCertPassword);

            // Create the email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Secure Message";
            message.Body = "This message is encrypted and digitally signed.";

            // Encrypt the message using the recipient's public certificate
            MailMessage encryptedMessage = message.Encrypt(publicCert);
            Console.WriteLine(encryptedMessage.IsEncrypted ? "Message encrypted." : "Encryption failed.");

            // Sign the encrypted message using the sender's private certificate
            MailMessage signedEncryptedMessage = encryptedMessage.AttachSignature(signingCert);
            Console.WriteLine(signedEncryptedMessage.IsSigned ? "Message signed." : "Signing failed.");

            // Ensure output directory exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the signed and encrypted message to a MSG file
            string outputPath = Path.Combine(outputDir, "SignedEncryptedMessage.msg");
            signedEncryptedMessage.Save(outputPath);
            Console.WriteLine($"Signed and encrypted message saved to: {outputPath}");

            // SMTP client configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "smtp_user";
            string smtpPassword = "smtp_password";


            // Skip external calls when placeholder credentials are used
            if (signingCertPassword == "password" || smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Send the message securely via SMTP
            using (SmtpClient smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;
                    smtpClient.SecurityOptions = SecurityOptions.Auto;
                    smtpClient.Username = smtpUsername;
                    smtpClient.Password = smtpPassword;

                    smtpClient.Send(signedEncryptedMessage);
                    Console.WriteLine("Message sent successfully via SMTP.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                }
            }

            // Dispose of messages
            message.Dispose();
            encryptedMessage.Dispose();
            signedEncryptedMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
