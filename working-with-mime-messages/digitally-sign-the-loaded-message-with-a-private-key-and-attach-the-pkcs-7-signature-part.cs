using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath = "input.eml";
            string signedPath = "signed.eml";
            string certificatePath = "privateCert.pfx";
            string certificatePassword = "password";

            // Ensure the input message file exists
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                // Create a minimal placeholder message
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                {
                    placeholder.Save(messagePath);
                }
            }

            // Ensure the certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the message
            using (MailMessage message = MailMessage.Load(messagePath))
            {
                // Load the certificate (private key)
                X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

                // Sign the message using SecureEmailManager
                SecureEmailManager manager = new SecureEmailManager();
                MailMessage signedMessage = manager.AttachSignature(message, certificate);

                // Save the signed message
                signedMessage.Save(signedPath);
                Console.WriteLine($"Signed message saved to: {signedPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
