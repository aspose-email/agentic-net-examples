using System;
using System.IO;
using Aspose.Email;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the certificate and the signed output
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";
            string outputPath = "signedMessage.eml";

            // Verify that the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the X.509 certificate
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certificatePath, certificatePassword);
            }
            catch (Exception certEx)
            {
                Console.Error.WriteLine($"Failed to load certificate: {certEx.Message}");
                return;
            }

            // Create a simple mail message
            MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Signed Email", "This is a signed email.");

            // Attach a detached digital signature
            MailMessage signedMessage;
            try
            {
                signedMessage = message.AttachSignature(certificate, true);
            }
            catch (Exception signEx)
            {
                Console.Error.WriteLine($"Failed to attach signature: {signEx.Message}");
                return;
            }

            // Save the signed message to a file
            try
            {
                signedMessage.Save(outputPath);
                Console.WriteLine($"Signed message saved to {outputPath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save signed message: {saveEx.Message}");
            }
            finally
            {
                // Dispose resources
                message.Dispose();
                signedMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
