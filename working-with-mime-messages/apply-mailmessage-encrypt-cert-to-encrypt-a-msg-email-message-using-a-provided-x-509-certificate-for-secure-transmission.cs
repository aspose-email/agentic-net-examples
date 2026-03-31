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
            // Path to the X.509 certificate
            string certificatePath = "publicCert.cer";
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            X509Certificate2 publicCertificate;
            try
            {
                publicCertificate = new X509Certificate2(certificatePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Path to the input MSG file
            string inputMsgPath = "input.msg";
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

                // Create a minimal placeholder MSG if it does not exist
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                {
                    try
                    {
                        placeholder.Save(inputMsgPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                        return;
                    }
                }
            }

            MailMessage message;
            try
            {
                message = MailMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                MailMessage encryptedMessage;
                try
                {
                    encryptedMessage = message.Encrypt(publicCertificate);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Encryption failed: {ex.Message}");
                    return;
                }

                using (encryptedMessage)
                {
                    string outputMsgPath = "encrypted.msg";
                    try
                    {
                        encryptedMessage.Save(outputMsgPath);
                        Console.WriteLine($"Encrypted message saved to {outputMsgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save encrypted MSG: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
