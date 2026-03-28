using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";
            string signedMsgPath = "signedMessage.msg";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the X.509 certificate
            using (X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword))
            {
                // Create a simple email message
                using (MailMessage mail = new MailMessage("sender@example.com", "receiver@example.com", "Signed message", "This is a signed email."))
                {
                    // Sign the message (detached = false)
                    MailMessage signedMail = mail.AttachSignature(certificate, false);

                    // Convert the signed MailMessage to a MapiMessage
                    using (MapiMessage mapi = MapiMessage.FromMailMessage(signedMail))
                    {
                        // Ensure the output directory exists
                        string outputDirectory = Path.GetDirectoryName(signedMsgPath);
                        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        // Save the signed message as MSG
                        mapi.Save(signedMsgPath);
                    }
                }
            }

            // Verify that the signed MSG file exists before loading
            if (!File.Exists(signedMsgPath))
            {
                Console.Error.WriteLine($"Signed MSG file not found: {signedMsgPath}");
                return;
            }

            // Load the MSG file and validate its signature
            using (MapiMessage loadedMsg = MapiMessage.Load(signedMsgPath))
            {
                try
                {
                    X509Certificate2[] signers = loadedMsg.CheckSignature();
                    Console.WriteLine($"Signature is valid. Number of signers: {signers.Length}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Signature verification failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
