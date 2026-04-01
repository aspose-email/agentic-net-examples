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
            string pfxPath = "certificate.pfx";
            string pfxPassword = "yourPassword";
            string msgPath = "input.msg";
            string outputPath = "encrypted.msg";

            // Ensure input files exist
            if (!File.Exists(pfxPath))
            {
                // Create a minimal placeholder PFX (empty content)
                File.WriteAllBytes(pfxPath, new byte[0]);
                Console.Error.WriteLine($"Placeholder PFX created at '{pfxPath}'.");
            }

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                // Create a minimal placeholder MSG file
                using (MapiMessage placeholderMsg = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder Body"))
                {
                    placeholderMsg.Save(msgPath);
                }
                Console.Error.WriteLine($"Placeholder MSG created at '{msgPath}'.");
            }

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage for encryption
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Load the certificate
                    X509Certificate2 certificate;
                    try
                    {
                        certificate = new X509Certificate2(pfxPath, pfxPassword);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                        return;
                    }

                    // Encrypt the message with the certificate
                    MailMessage encryptedMessage = mailMessage.Encrypt(certificate);

                    // Convert back to MapiMessage
                    using (MapiMessage encryptedMapi = MapiMessage.FromMailMessage(encryptedMessage))
                    {
                        // Ensure output directory exists
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the encrypted MSG
                        encryptedMapi.Save(outputPath);
                        Console.WriteLine($"Encrypted MSG saved to '{outputPath}'.");
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
