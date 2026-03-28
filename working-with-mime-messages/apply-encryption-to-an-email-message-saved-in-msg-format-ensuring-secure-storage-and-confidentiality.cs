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
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the certificate
            X509Certificate2 publicCertificate = new X509Certificate2(certificatePath);

            // Destination MSG file
            string outputMsgPath = "encryptedMessage.msg";
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create the original mail message
            using (MailMessage originalMessage = new MailMessage())
            {
                originalMessage.From = "sender@example.com";
                originalMessage.To = "recipient@example.com";
                originalMessage.Subject = "Secure Message";
                originalMessage.Body = "This is a confidential email.";

                // Encrypt the message with the public certificate
                using (MailMessage encryptedMessage = originalMessage.Encrypt(publicCertificate))
                {
                    // Save the encrypted message as MSG (Unicode) format
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    encryptedMessage.Save(outputMsgPath, saveOptions);
                }
            }

            Console.WriteLine("Message encrypted and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
