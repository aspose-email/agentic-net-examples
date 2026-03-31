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
            // Define file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "signed_output.msg";
            string certificatePath = "certificate.pfx";
            string certificatePassword = "password";

            // Ensure input MSG exists; create a minimal placeholder if missing
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

                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body"))
                {
                    placeholder.Save(inputMsgPath);
                }
            }

            // Ensure certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage mapimsg = MapiMessage.Load(inputMsgPath))
            {
                // Convert to MailMessage with explicit MailConversionOptions (required for signature tasks)
                MailMessage mailMessage = mapimsg.ToMailMessage(new MailConversionOptions());

                // Load the signing certificate
                using (X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword))
                {
                    // Attach digital signature
                    SecureEmailManager secureManager = new SecureEmailManager();
                    MailMessage signedMail = secureManager.AttachSignature(mailMessage, certificate);

                    // Convert back to MapiMessage and save
                    using (MapiMessage signedMapi = MapiMessage.FromMailMessage(signedMail))
                    {
                        signedMapi.Save(outputMsgPath);
                    }
                }
            }

            Console.WriteLine("Message signed and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
