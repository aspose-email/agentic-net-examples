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
            // Define file paths
            string dataDir = Path.Combine(Environment.CurrentDirectory, "Data");
            string inputMsgPath = Path.Combine(dataDir, "input.msg");
            string signedMsgPath = Path.Combine(dataDir, "signed.msg");
            string certPath = Path.Combine(dataDir, "certificate.pfx");
            string certPassword = "password";

            // Ensure the data directory exists
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            // Guard certificate file existence
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Guard input MSG file existence; create a minimal placeholder if missing
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
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder Body"))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Load the certificate
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certPath, certPassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Sign the message
            SecureEmailManager manager = new SecureEmailManager();
            MapiMessage signedMsg;
            try
            {
                signedMsg = manager.AttachSignature(msg, certificate);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to sign the message: {ex.Message}");
                return;
            }

            // Save the signed MSG
            try
            {
                signedMsg.Save(signedMsgPath);
                Console.WriteLine($"Signed message saved to: {signedMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save signed MSG: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
