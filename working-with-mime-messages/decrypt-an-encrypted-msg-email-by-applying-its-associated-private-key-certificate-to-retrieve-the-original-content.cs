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
            // Input MSG file path
            string inputMsgPath = "encrypted.msg";
            // Output MSG file path
            string outputMsgPath = "decrypted.msg";
            // Private key certificate path (PFX) and password
            string certificatePath = "privateCert.pfx";
            string certificatePassword = "password";

            // Verify input files exist
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the private key certificate
            using (X509Certificate2 privateCertificate = new X509Certificate2(certificatePath, certificatePassword))
            {
                // Load the encrypted MSG message
                using (MapiMessage encryptedMessage = MapiMessage.Load(inputMsgPath))
                {
                    if (!encryptedMessage.IsEncrypted)
                    {
                        Console.WriteLine("The message is not encrypted. No decryption needed.");
                        return;
                    }

                    // Decrypt the message using the private certificate
                    MapiMessage decryptedMessage = encryptedMessage.Decrypt(privateCertificate);

                    // Save the decrypted message
                    decryptedMessage.Save(outputMsgPath);
                    Console.WriteLine($"Decrypted message saved to: {outputMsgPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
