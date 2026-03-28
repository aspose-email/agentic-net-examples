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
            // Paths to the source MSG file, the certificate and the output signed MSG file
            string msgFilePath = "input.msg";
            string signedMsgFilePath = "signed.msg";
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";

            // Verify that the source MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Source MSG file not found: {msgFilePath}");
                return;
            }

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the certificate (private key required for signing)
            X509Certificate2 signingCertificate = new X509Certificate2(certificatePath, certificatePassword);

            // Load the MSG message
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Create the manager that handles secure operations
                SecureEmailManager secureManager = new SecureEmailManager();

                // Attach a digital signature to the message
                MapiMessage signedMessage = secureManager.AttachSignature(message, signingCertificate);

                // Save the signed message to a new file
                signedMessage.Save(signedMsgFilePath);
                signedMessage.Dispose();

                Console.WriteLine($"Message signed successfully and saved to: {signedMsgFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
