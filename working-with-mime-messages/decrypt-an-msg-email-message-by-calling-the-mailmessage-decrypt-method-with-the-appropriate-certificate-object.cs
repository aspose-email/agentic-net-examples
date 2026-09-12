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
            // Author note: This sample demonstrates loading an encrypted MSG file,
            // decrypting it with a certificate, and saving the result as an EML file.

            string msgPath = "encrypted.msg";
            string certPath = "privateKey.pfx";
            string outputPath = "decrypted.eml";

            // Verify input files exist
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

                Console.Error.WriteLine($"Input MSG file not found: {msgPath}");
                return;
            }
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Load the certificate (assumes no password; adjust if needed)
            X509Certificate2 certificate = new X509Certificate2(certPath);

            // Load the encrypted MSG as a MapiMessage
            MapiMessage mapMsg = MapiMessage.Load(msgPath);

            // Convert MapiMessage to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions))
            {
                // Decrypt the MailMessage using the provided certificate
                using (MailMessage decryptedMsg = mailMsg.Decrypt(certificate))
                {
                    // Save the decrypted message to an EML file
                    decryptedMsg.Save(outputPath);
                    Console.WriteLine($"Decrypted message saved to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
