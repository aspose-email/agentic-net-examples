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
            // Paths to the encrypted MSG file, certificate file and output file
            string msgPath = "encrypted.msg";
            string certPath = "mycert.pfx";
            string outputPath = "decrypted.msg";

            // Verify that the input files exist
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Load the certificate (replace the password with the actual one if needed)
            string certPassword = "password";
            using (X509Certificate2 certificate = new X509Certificate2(certPath, certPassword))
            {
                // Load the encrypted MSG message
                using (MailMessage encryptedMessage = MailMessage.Load(msgPath))
                {
                    // Decrypt the message using the certificate
                    using (MailMessage decryptedMessage = encryptedMessage.Decrypt(certificate))
                    {
                        // Save the decrypted message
                        decryptedMessage.Save(outputPath);
                        Console.WriteLine($"Message decrypted and saved to {outputPath}");
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
