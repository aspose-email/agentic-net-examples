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
            string certificatePath = "MartinCertificate.cer";
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine("Certificate file not found: " + certificatePath);
                return;
            }

            using (X509Certificate2 publicCertificate = new X509Certificate2(certificatePath))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = "atneostthaecrcount@gmail.com";
                    message.To = "atneostthaecrcount@gmail.com";
                    message.Subject = "Test subject";
                    message.Body = "Test Body";

                    using (MailMessage encryptedMessage = message.Encrypt(publicCertificate))
                    {
                        Console.WriteLine(encryptedMessage.IsEncrypted ? "Its encrypted" : "Its NOT encrypted");

                        string outputPath = "encrypted.msg";
                        try
                        {
                            encryptedMessage.Save(outputPath);
                            Console.WriteLine("Encrypted message saved to: " + outputPath);
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine("Failed to save encrypted message: " + ioEx.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
