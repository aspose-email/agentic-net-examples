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
            // Author note: This sample demonstrates decrypting an encrypted MSG file using a private key certificate.
            string encryptedMsgPath = "encrypted.msg";
            string privateCertPath = "privateKey.pfx";
            string certPassword = "yourPassword";

            // Verify input files exist
            if (!File.Exists(encryptedMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(encryptedMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Encrypted MSG file not found: {encryptedMsgPath}");
                return;
            }

            if (!File.Exists(privateCertPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {privateCertPath}");
                return;
            }

            // Load the private key certificate
            X509Certificate2 privateCertificate = new X509Certificate2(privateCertPath, certPassword);

            // Load the encrypted MSG message
            MapiMessage encryptedMapiMessage = MapiMessage.Load(encryptedMsgPath);

            // Decrypt the message using the private certificate
            MapiMessage decryptedMapiMessage = encryptedMapiMessage.Decrypt(privateCertificate);

            // Convert to MailMessage for easier handling and saving
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage decryptedMailMessage = decryptedMapiMessage.ToMailMessage(conversionOptions);

            // Save the decrypted message as an EML file
            string outputEmlPath = "decrypted.eml";
            using (decryptedMailMessage)
            {
                decryptedMailMessage.Save(outputEmlPath);
            }

            Console.WriteLine($"Decryption successful. Decrypted message saved to: {outputEmlPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
