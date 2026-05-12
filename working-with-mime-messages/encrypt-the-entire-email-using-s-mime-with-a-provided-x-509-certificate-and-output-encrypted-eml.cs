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
            string certificateFile = "certificate.cer";
            if (!File.Exists(certificateFile))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificateFile}");
                return;
            }

            string outputFile = "encrypted.eml";
            string outputDirectory = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (X509Certificate2 certificate = new X509Certificate2(certificateFile))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Encrypted Message";
                    message.Body = "This is a secret message.";

                    using (MailMessage encryptedMessage = message.Encrypt(certificate))
                    {
                        encryptedMessage.Save(outputFile);
                        Console.WriteLine(encryptedMessage.IsEncrypted ? "Message encrypted successfully." : "Message encryption failed.");
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error: {exception.Message}");
        }
    }
}
