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
            // Paths for certificate and signed MSG file
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";
            string signedMsgPath = "signedMessage.msg";

            // Verify certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine("Certificate file not found: " + certificatePath);
                return;
            }

            // Load the certificate
            X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(signedMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a simple email message
            using (MailMessage mail = new MailMessage("sender@example.com", "receiver@example.com", "Signed Email", "This is a signed email."))
            {
                // Sign the message using SecureEmailManager
                SecureEmailManager securityManager = new SecureEmailManager();
                using (MailMessage signedMail = securityManager.AttachSignature(mail, certificate))
                {
                    // Convert signed MailMessage to MapiMessage and save as MSG
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(signedMail))
                    {
                        mapiMessage.Save(signedMsgPath);
                    }
                }
            }

            // Verify that the signed MSG file was created
            if (!File.Exists(signedMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(signedMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("Signed MSG file not found: " + signedMsgPath);
                return;
            }

            // Load the MSG file
            using (MapiMessage loadedMapi = MapiMessage.Load(signedMsgPath))
            {
                // Convert to MailMessage with MailConversionOptions as required
                MailMessage loadedMail = loadedMapi.ToMailMessage(new MailConversionOptions());

                // Verify the signature
                SecureEmailManager verificationManager = new SecureEmailManager();
                SmimeResult verificationResult = verificationManager.CheckSignature(loadedMail);

                Console.WriteLine(verificationResult.IsSuccess ? "Signature is valid." : "Signature is invalid.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
