using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "encryptedMessage.msg";
            string certFilePath = "privateKey.pfx";
            string certPassword = "password";

            // Verify that the MSG file exists
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Verify that the certificate file exists
            if (!File.Exists(certFilePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certFilePath}");
                return;
            }

            // Load the certificate containing the private key
            using (X509Certificate2 privateCertificate = new X509Certificate2(certFilePath, certPassword))
            {
                // Load the encrypted MSG file
                using (MapiMessage encryptedMessage = MapiMessage.Load(msgFilePath))
                {
                    // Decrypt the message using the provided certificate
                    MapiMessage decryptedMessage = encryptedMessage.Decrypt(privateCertificate);

                    // Output basic information from the decrypted message
                    Console.WriteLine($"Subject: {decryptedMessage.Subject}");
                    Console.WriteLine($"From: {decryptedMessage.SenderName}");
                    Console.WriteLine("Body:");
                    Console.WriteLine(decryptedMessage.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
