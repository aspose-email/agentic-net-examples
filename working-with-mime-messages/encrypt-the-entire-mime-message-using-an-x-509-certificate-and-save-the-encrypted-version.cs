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
            string certificatePath = "publicCert.cer";
            string encryptedMessagePath = "encryptedMessage.eml";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(encryptedMessagePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the X.509 certificate
            using (X509Certificate2 publicCertificate = new X509Certificate2(certificatePath))
            {
                // Create the original mail message
                using (MailMessage originalMessage = new MailMessage())
                {
                    originalMessage.From = "sender@example.com";
                    originalMessage.To.Add("receiver@example.com");
                    originalMessage.Subject = "Encrypted Message";
                    originalMessage.Body = "This is a secret message.";

                    // Encrypt the message using the certificate
                    using (MailMessage encryptedMessage = originalMessage.Encrypt(publicCertificate))
                    {
                        // Save the encrypted message to a file
                        try
                        {
                            encryptedMessage.Save(encryptedMessagePath);
                            Console.WriteLine($"Encrypted message saved to {encryptedMessagePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save encrypted message: {saveEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
