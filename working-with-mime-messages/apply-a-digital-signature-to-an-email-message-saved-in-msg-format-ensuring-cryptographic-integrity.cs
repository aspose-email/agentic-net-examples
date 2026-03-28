using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";
            string outputMsgPath = "signedMessage.msg";

            // Verify the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the X.509 certificate
            using (X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword))
            {
                // Create a simple mail message
                using (MailMessage mailMessage = new MailMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Signed MSG",
                    "This is a signed message."))
                {
                    // Sign the message using SecureEmailManager
                    SecureEmailManager secureManager = new SecureEmailManager();
                    MailMessage signedMail = secureManager.AttachSignature(mailMessage, certificate);

                    // Convert the signed MailMessage to MapiMessage for MSG format
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(signedMail))
                    {
                        // Save the signed message as MSG
                        mapiMessage.Save(outputMsgPath);
                        Console.WriteLine($"Signed MSG saved to: {outputMsgPath}");
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
