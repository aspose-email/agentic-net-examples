using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the public certificate file
            string certPath = "MartinCertificate.cer";

            // Verify that the certificate file exists
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Load the X509 certificate
            X509Certificate2 publicCert = new X509Certificate2(certPath);

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = "atneostthaecrcount@gmail.com";
            message.To = "atneostthaecrcount@gmail.com";
            message.Subject = "Test subject";
            message.Body = "Test Body";

            // Encrypt the message using the certificate
            MailMessage encryptedMessage = message.Encrypt(publicCert);
            Console.WriteLine(encryptedMessage.IsEncrypted ? "Its encrypted" : "Its NOT encrypted");

            // Define output MSG file path
            string outputPath = "EncryptedMessage.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the encrypted message as MSG
            try
            {
                encryptedMessage.Save(outputPath);
                Console.WriteLine($"Encrypted message saved to: {outputPath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to save encrypted message: {ioEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
