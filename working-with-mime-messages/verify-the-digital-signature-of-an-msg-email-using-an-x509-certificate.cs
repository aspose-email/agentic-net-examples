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
            // Paths to the MSG file and the certificate file.
            string msgFilePath = "sample.msg";
            string certFilePath = "certificate.pfx";
            string certPassword = "password";

            // Verify that the MSG file exists; if not, create a minimal placeholder.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage("placeholder@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                    Console.WriteLine($"Placeholder MSG created at '{msgFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Verify that the certificate file exists.
            if (!File.Exists(certFilePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certFilePath}");
                return;
            }

            // Load the certificate.
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certFilePath, certPassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Load the MSG file into a MapiMessage.
            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                // Convert the MapiMessage to a MailMessage with default conversion options.
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Create SecureEmailManager to check the signature.
                    SecureEmailManager secureManager = new SecureEmailManager();

                    // Perform the signature check using the provided certificate.
                    SmimeResult smimeResult = secureManager.CheckSignature(mailMessage, certificate);

                    // Use the correct property to determine success.
                    bool isValid = smimeResult.IsSuccess;

                    Console.WriteLine(isValid
                        ? "The message signature is valid."
                        : "The message signature is NOT valid.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
