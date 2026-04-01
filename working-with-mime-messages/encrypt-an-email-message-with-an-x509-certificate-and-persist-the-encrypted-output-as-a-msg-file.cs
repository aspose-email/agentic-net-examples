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
            // Paths for the certificate and the output MSG file
            string certificatePath = "publicCert.cer";
            string outputMsgPath = "encrypted.msg";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the X509 certificate
            X509Certificate2 certificate = new X509Certificate2(certificatePath);

            // Create a simple email message
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "receiver@example.com",
                "Encrypted Message",
                "This is a secret."))
            {
                // Encrypt the message with the certificate
                MailMessage encryptedMessage = message.Encrypt(certificate);

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the encrypted message as MSG
                encryptedMessage.Save(outputMsgPath);
                Console.WriteLine($"Encrypted message saved to: {outputMsgPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
