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
            // Path to the private certificate (PFX) and its password
            string certificatePath = "privateCert.pfx";
            string certificatePassword = "password";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the certificate
            using (X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword))
            {
                // Create a simple mail message
                using (MailMessage message = new MailMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Signed Email",
                    "This is a signed email."))
                {
                    // Apply a digital signature using SecureEmailManager
                    SecureEmailManager manager = new SecureEmailManager();
                    using (MailMessage signedMessage = manager.AttachSignature(message, certificate))
                    {
                        // Define output path for the signed message
                        string outputPath = "signedMessage.eml";

                        // Ensure the output directory exists
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the signed message
                        try
                        {
                            signedMessage.Save(outputPath);
                            Console.WriteLine($"Signed message saved to: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save signed message: {ex.Message}");
                        }
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
