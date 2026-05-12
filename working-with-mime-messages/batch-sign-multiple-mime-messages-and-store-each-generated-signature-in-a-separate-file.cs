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
            // Paths configuration
            string inputDirectory = "InputMails";
            string outputDirectory = "SignedMails";
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";

            // Verify input directory
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Verify certificate file
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load certificate
            X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

            // SecureEmailManager for signing
            SecureEmailManager secureManager = new SecureEmailManager();

            // Process each .eml file in the input directory
            string[] emlFiles = Directory.GetFiles(inputDirectory, "*.eml");
            foreach (string emlFilePath in emlFiles)
            {
                try
                {
                    // Load the original MIME message
                    using (MailMessage originalMessage = MailMessage.Load(emlFilePath))
                    {
                        // Attach digital signature
                        MailMessage signedMessage = secureManager.AttachSignature(originalMessage, certificate);

                        // Prepare output file path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(emlFilePath);
                        string signedFilePath = Path.Combine(outputDirectory, fileNameWithoutExt + "_signed.eml");

                        // Save the signed message
                        signedMessage.Save(signedFilePath);
                        Console.WriteLine($"Signed message saved to: {signedFilePath}");

                        // Dispose signed message
                        signedMessage.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{emlFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
