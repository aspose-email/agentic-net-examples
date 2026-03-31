using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputMsgPath = "encrypted.msg";
            string certPath = "public.cer";

            // Ensure input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputMsgPath);
                        Console.WriteLine($"Placeholder MSG created at '{inputMsgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure certificate file exists
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file '{certPath}' not found. Cannot encrypt message.");
                return;
            }

            // Load the certificate
            X509Certificate2 publicCert;
            try
            {
                publicCert = new X509Certificate2(certPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Load the MSG file, convert to MailMessage, encrypt, and save
            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                using (MailMessage mail = msg.ToMailMessage(new MailConversionOptions()))
                using (MailMessage encryptedMail = mail.Encrypt(publicCert))
                using (MapiMessage encryptedMsg = MapiMessage.FromMailMessage(encryptedMail))
                {
                    encryptedMsg.Save(outputMsgPath);
                    Console.WriteLine(encryptedMail.IsEncrypted
                        ? $"Message encrypted successfully and saved to '{outputMsgPath}'."
                        : "Encryption failed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during encryption process: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
