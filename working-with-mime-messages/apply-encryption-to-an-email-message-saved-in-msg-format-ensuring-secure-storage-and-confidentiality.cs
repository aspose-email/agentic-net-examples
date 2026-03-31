using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

namespace AsposeEmailEncryptionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define file paths
                string inputMsgPath = "input.msg";
                string outputMsgPath = "encrypted.msg";
                string certificatePath = "public.cer";

                // Ensure input MSG exists; create a minimal placeholder if missing
                if (!File.Exists(inputMsgPath))
                {
                    // Ensure the directory for the input file exists
                    string inputDirectory = Path.GetDirectoryName(inputMsgPath);
                    if (!string.IsNullOrEmpty(inputDirectory) && !Directory.Exists(inputDirectory))
                    {
                        Directory.CreateDirectory(inputDirectory);
                    }

                    // Create a simple placeholder mail message
                    using (MailMessage placeholderMessage = new MailMessage())
                    {
                        placeholderMessage.From = "placeholder@example.com";
                        placeholderMessage.To.Add("placeholder@example.com");
                        placeholderMessage.Subject = "Placeholder";
                        placeholderMessage.Body = "This is a placeholder message.";

                        // Save the placeholder as MSG (Unicode format)
                        MsgSaveOptions placeholderSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        placeholderMessage.Save(inputMsgPath, placeholderSaveOptions);
                    }
                }

                // Verify that the certificate file exists
                if (!File.Exists(certificatePath))
                {
                    Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                    return;
                }

                // Load the mail message from the MSG file
                using (MailMessage mailMessage = MailMessage.Load(inputMsgPath, new MsgLoadOptions()))
                {
                    // Load the X509 certificate
                    using (X509Certificate2 certificate = new X509Certificate2(certificatePath))
                    {
                        // Encrypt the message using the certificate
                        MailMessage encryptedMessage = mailMessage.Encrypt(certificate);

                        // Ensure the output directory exists
                        string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        // Save the encrypted message as MSG (Unicode format)
                        MsgSaveOptions encryptedSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        encryptedMessage.Save(outputMsgPath, encryptedSaveOptions);

                        // Dispose the encrypted message
                        encryptedMessage.Dispose();

                        Console.WriteLine($"Encrypted message saved to: {outputMsgPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
