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
            // Paths to the encrypted MSG file and the certificate file
            string msgPath = "encryptedMessage.msg";
            string certPath = "mycert.pfx";
            string certPassword = "password";

            // Verify that the message file exists
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Verify that the certificate file exists
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Load the X509 certificate
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certPath, certPassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Load the encrypted message
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Decrypt the message using the certificate
                using (MailMessage decryptedMessage = mailMessage.Decrypt(certificate))
                {
                    // Save the decrypted message to a new file
                    string decryptedPath = "decryptedMessage.eml";
                    try
                    {
                        decryptedMessage.Save(decryptedPath);
                        Console.WriteLine($"Decrypted message saved to {decryptedPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save decrypted message: {ex.Message}");
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
