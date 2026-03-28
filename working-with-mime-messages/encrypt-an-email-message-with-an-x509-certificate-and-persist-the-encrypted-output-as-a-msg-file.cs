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
            // Path to the public certificate used for encryption
            string certificatePath = "publicCert.cer";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the X509 certificate
            X509Certificate2 publicCertificate = new X509Certificate2(certificatePath);

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Encrypted message";
                message.Body = "This is a secret.";

                // Encrypt the message with the certificate
                using (MailMessage encryptedMessage = message.Encrypt(publicCertificate))
                {
                    // Destination path for the encrypted MSG file
                    string outputPath = "encryptedMessage.msg";

                    // Ensure the output directory exists
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Save the encrypted message as MSG
                    encryptedMessage.Save(outputPath);

                    Console.WriteLine(encryptedMessage.IsEncrypted
                        ? "Message encrypted and saved successfully."
                        : "Encryption failed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
