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
            string inputFolder = "InputMessages";
            string outputFolder = "EncryptedMessages";
            string certificatePath = "publicCert.cer";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            // Verify certificate file exists and load it
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file '{certificatePath}' not found.");
                return;
            }

            using (X509Certificate2 certificate = new X509Certificate2(certificatePath))
            {
                string[] emlFiles;
                try
                {
                    emlFiles = Directory.GetFiles(inputFolder, "*.eml");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate .eml files: {ex.Message}");
                    return;
                }

                foreach (string emlPath in emlFiles)
                {
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

                        Console.Error.WriteLine($"File '{emlPath}' does not exist, skipping.");
                        continue;
                    }

                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        MailMessage encryptedMessage = message.Encrypt(certificate);
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(emlPath);
                        string encryptedPath = Path.Combine(outputFolder, fileNameWithoutExt + "_encrypted.eml");

                        try
                        {
                            encryptedMessage.Save(encryptedPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save encrypted message '{encryptedPath}': {ex.Message}");
                        }

                        encryptedMessage.Dispose();
                    }
                }
            }

            Console.WriteLine("Batch encryption completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
