using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

class Program
{
    static void Main()
    {
        // Paths and credentials
        const string certificatePath = "certificate.pfx";
        const string certificatePassword = "yourPassword";
        const string outputMsgPath = "signedMessage.msg";

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

        try
        {
            // Load the X509 certificate from the PFX file
            X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

            // Create the original mail message
            using (MailMessage originalMessage = new MailMessage(
                "sender@example.com",
                "receiver@example.com",
                "Signed Message",
                "This is a signed email."))
            {
                // Attach a digital signature (detached = false)
                using (MailMessage signedMessage = originalMessage.AttachSignature(certificate, false))
                {
                    // Save the signed message as MSG
                    signedMessage.Save(outputMsgPath);
                    Console.WriteLine($"Signed message saved to: {outputMsgPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
