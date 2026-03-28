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
            // Paths to the input MSG file, certificate file, and output signed MSG file
            string inputMsgPath = "input.msg";
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";
            string outputMsgPath = "signed.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the certificate
            using (X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword))
            {
                // Load the original message
                using (MailMessage originalMessage = MailMessage.Load(inputMsgPath))
                {
                    // Apply a digital signature (creates a signed copy)
                    using (MailMessage signedMessage = originalMessage.AttachSignature(certificate))
                    {
                        // Save the signed message
                        signedMessage.Save(outputMsgPath);
                        Console.WriteLine($"Signed message saved to: {outputMsgPath}");
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
