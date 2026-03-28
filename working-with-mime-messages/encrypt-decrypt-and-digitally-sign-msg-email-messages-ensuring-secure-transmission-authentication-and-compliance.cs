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
            // Paths for certificates and message files
            const string publicCertPath = "public.cer";
            const string privateCertPath = "private.pfx";
            const string privateCertPassword = "password";
            const string encryptedMsgPath = "encrypted.msg";
            const string signedMsgPath = "signed.msg";

            // Verify certificate files exist
            if (!File.Exists(publicCertPath))
            {
                Console.Error.WriteLine($"Public certificate not found: {publicCertPath}");
                return;
            }
            if (!File.Exists(privateCertPath))
            {
                Console.Error.WriteLine($"Private certificate not found: {privateCertPath}");
                return;
            }

            // Load certificates
            X509Certificate2 publicCert = new X509Certificate2(publicCertPath);
            X509Certificate2 privateCert = new X509Certificate2(privateCertPath, privateCertPassword);

            // Create a simple mail message
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "receiver@example.com",
                "Secure Email Sample",
                "This message will be encrypted, decrypted, signed and verified."))
            {
                // Encrypt the message with the public certificate
                MailMessage encryptedMessage = message.Encrypt(publicCert);

                // Save the encrypted message
                encryptedMessage.Save(encryptedMsgPath);
                Console.WriteLine($"Encrypted message saved to {encryptedMsgPath}");

                // Load the encrypted message from file
                using (MailMessage loadedEncrypted = MailMessage.Load(encryptedMsgPath))
                {
                    // Decrypt using the private certificate
                    MailMessage decryptedMessage = loadedEncrypted.Decrypt(privateCert);
                    Console.WriteLine("Message decrypted successfully.");

                    // Sign the decrypted message using the private certificate
                    SecureEmailManager secMgr = new SecureEmailManager();
                    MailMessage signedMessage = secMgr.AttachSignature(decryptedMessage, privateCert);
                    signedMessage.Save(signedMsgPath);
                    Console.WriteLine($"Signed message saved to {signedMsgPath}");

                    // Verify the signature
                    SmimeResult verifyResult = secMgr.CheckSignature(signedMessage);
                    bool isSignatureValid = verifyResult.IsSuccess && verifyResult.SigningCertificates.Count > 0;
                    Console.WriteLine(isSignatureValid
                        ? "Signature verification succeeded."
                        : "Signature verification failed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
