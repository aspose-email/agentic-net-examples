using System;
using System.IO;
using Aspose.Email;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "signedMessage.eml";
            string certPath = "publicCert.cer";

            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input EML file not found: {emlPath}");
                return;
            }

            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Public certificate file not found: {certPath}");
                return;
            }

            try
            {
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                using (X509Certificate2 publicCertificate = new X509Certificate2(certPath))
                {
                    SecureEmailManager manager = new SecureEmailManager();

                    SmimeResult checkResult = manager.CheckSignature(mailMessage, publicCertificate);

                    if (checkResult != null && checkResult.IsSuccess)
                    {
                        Console.WriteLine("Signature is valid.");
                    }
                    else
                    {
                        string errorMessage = checkResult?.Error?.Message ?? "Signature verification failed.";
                        Console.WriteLine(errorMessage);
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error processing files: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
