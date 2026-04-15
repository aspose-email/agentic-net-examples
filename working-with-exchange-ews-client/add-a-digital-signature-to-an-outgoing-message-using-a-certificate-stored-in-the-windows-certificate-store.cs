using System;
using Aspose.Email;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            // Open the current user's personal certificate store.
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);

                // Find a valid certificate that contains a private key.
                X509Certificate2Collection validCertificates = store.Certificates.Find(
                    X509FindType.FindByTimeValid, DateTime.Now, false);

                X509Certificate2 signingCertificate = null;
                foreach (X509Certificate2 cert in validCertificates)
                {
                    if (cert.HasPrivateKey)
                    {
                        signingCertificate = cert;
                        break;
                    }
                }

                if (signingCertificate == null)
                {
                    Console.Error.WriteLine("No suitable signing certificate found in the store.");
                    return;
                }

                // Create the email message.
                using (MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Signed Email",
                    "This email is digitally signed."))
                {
                    // Attach a digital signature.
                    SecureEmailManager secureManager = new SecureEmailManager();
                    MailMessage signedMessage = secureManager.AttachSignature(message, signingCertificate);

                    using (signedMessage)
                    {
                        Console.WriteLine(signedMessage.IsSigned
                            ? "Message signed successfully."
                            : "Message signing failed.");
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
