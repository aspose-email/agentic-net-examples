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
            string inputMsgPath = "input.msg";
            string encryptedMsgPath = "encrypted.msg";
            string decryptedMsgPath = "decrypted.msg";
            string signedMsgPath = "signed.eml";
            string publicCertPath = "public.cer";
            string privateCertPath = "private.pfx";
            string privateCertPassword = "password";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (var placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                {
                    var msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    placeholder.Save(inputMsgPath, msgSaveOptions);
                }
            }

            // Load the message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            // Verify certificate files exist
            if (!File.Exists(publicCertPath) || !File.Exists(privateCertPath))
            {
                Console.Error.WriteLine("Certificate files are missing. Skipping encryption, decryption, and signing.");
                return;
            }

            // Load certificates
            X509Certificate2 publicCert;
            X509Certificate2 privateCert;
            try
            {
                publicCert = new X509Certificate2(publicCertPath);
                privateCert = new X509Certificate2(privateCertPath, privateCertPassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificates: {ex.Message}");
                return;
            }

            // Encrypt the message
            MailMessage encryptedMessage;
            try
            {
                encryptedMessage = message.Encrypt(publicCert);
                var encryptSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                encryptedMessage.Save(encryptedMsgPath, encryptSaveOptions);
                Console.WriteLine($"Encrypted message saved to {encryptedMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Encryption failed: {ex.Message}");
                return;
            }

            // Decrypt the message
            MailMessage decryptedMessage;
            try
            {
                decryptedMessage = encryptedMessage.Decrypt(privateCert);
                var decryptSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                decryptedMessage.Save(decryptedMsgPath, decryptSaveOptions);
                Console.WriteLine($"Decrypted message saved to {decryptedMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Decryption failed: {ex.Message}");
                return;
            }

            // Digitally sign the decrypted message
            try
            {
                var secureManager = new SecureEmailManager();
                MailMessage signedMessage = secureManager.AttachSignature(decryptedMessage, privateCert);

                // Save signed message preserving signature content
                var emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveSignedContent = true
                };
                signedMessage.Save(signedMsgPath, emlSaveOptions);
                Console.WriteLine($"Signed message saved to {signedMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Signing failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
