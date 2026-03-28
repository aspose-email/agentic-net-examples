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
            string inputMsgPath = "sample.msg";
            string outputMsgPath = "encrypted.msg";

            // Verify input files exist
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Message file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load certificate
            using (X509Certificate2 publicCertificate = new X509Certificate2(certificatePath))
            {
                // Load the MSG message
                using (MailMessage originalMessage = MailMessage.Load(inputMsgPath))
                {
                    // Encrypt the message with the certificate
                    using (MailMessage encryptedMessage = originalMessage.Encrypt(publicCertificate))
                    {
                        // Save the encrypted message
                        encryptedMessage.Save(outputMsgPath);
                        Console.WriteLine(encryptedMessage.IsEncrypted
                            ? "Message encrypted and saved successfully."
                            : "Encryption failed.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
