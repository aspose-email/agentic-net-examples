using System;
using System.IO;
using Aspose.Email;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string certificatePath = "recipient.cer";
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            string outputPath = "encrypted.eml";

            using (X509Certificate2 recipientCertificate = new X509Certificate2(certificatePath))
            {
                using (MailMessage originalMessage = new MailMessage())
                {
                    originalMessage.From = "sender@example.com";
                    originalMessage.To = "recipient@example.com";
                    originalMessage.Subject = "Encrypted Message";
                    originalMessage.Body = "This is a confidential message.";

                    MailMessage encryptedMessage = originalMessage.Encrypt(recipientCertificate);
                    try
                    {
                        encryptedMessage.Save(outputPath);
                        Console.WriteLine($"Message encrypted and saved to {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to save encrypted message: {ioEx.Message}");
                    }
                    finally
                    {
                        encryptedMessage.Dispose();
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
