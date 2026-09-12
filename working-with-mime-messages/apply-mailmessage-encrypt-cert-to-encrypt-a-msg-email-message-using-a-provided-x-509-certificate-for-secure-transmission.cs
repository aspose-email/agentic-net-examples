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
            // Paths for the certificate and the MSG file
            string certificatePath = "publicCert.cer";
            string messagePath = "input.msg";
            string encryptedMessagePath = "encrypted.msg";

            // Verify certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Verify MSG file exists
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {messagePath}");
                return;
            }

            // Load the X.509 certificate
            X509Certificate2 publicCertificate = new X509Certificate2(certificatePath);

            // Load the MSG file as a MapiMessage
            MapiMessage mapMessage = MapiMessage.Load(messagePath);

            // Convert MapiMessage to MailMessage
            MailMessage mailMessage = mapMessage.ToMailMessage(new MailConversionOptions());

            // Encrypt the MailMessage using the certificate
            MailMessage encryptedMessage = mailMessage.Encrypt(publicCertificate);

            // Output encryption status
            Console.WriteLine(encryptedMessage.IsEncrypted ? "Its encrypted" : "Its NOT encrypted");

            // Save the encrypted message
            try
            {
                encryptedMessage.Save(encryptedMessagePath);
                Console.WriteLine($"Encrypted message saved to: {encryptedMessagePath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save encrypted message: {saveEx.Message}");
            }

            // Dispose MailMessage objects
            mailMessage.Dispose();
            encryptedMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
