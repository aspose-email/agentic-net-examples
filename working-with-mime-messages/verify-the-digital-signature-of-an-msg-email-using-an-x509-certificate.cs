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
            // Paths to the MSG file and the certificate (PFX) file.
            const string msgPath = "sample.msg";
            const string certPath = "certificate.pfx";
            const string certPassword = "password";

            // Verify that the required files exist.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Load the certificate.
            using (X509Certificate2 certificate = new X509Certificate2(certPath, certPassword))
            {
                // Load the MSG file as a MapiMessage.
                MapiMessage mapiMsg = MapiMessage.Load(msgPath);

                // Convert the MapiMessage to a MailMessage using MailConversionOptions.
                MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                // Verify the signature of the MailMessage using the provided certificate.
                SecureEmailManager secMgr = new SecureEmailManager();
                SmimeResult verificationResult = secMgr.CheckSignature(mailMsg, certificate);

                // Output basic verification information.
                Console.WriteLine("Signature verification completed.");
                Console.WriteLine($"Result: {verificationResult}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
