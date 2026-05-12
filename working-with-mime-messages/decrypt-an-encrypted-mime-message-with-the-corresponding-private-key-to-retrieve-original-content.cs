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
            string inputPath = "encrypted.eml";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            using (MailMessage encryptedMessage = MailMessage.Load(inputPath))
            {
                using (MailMessage decryptedMessage = encryptedMessage.Decrypt())
                {
                    Console.WriteLine("Decrypted message body:");
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
