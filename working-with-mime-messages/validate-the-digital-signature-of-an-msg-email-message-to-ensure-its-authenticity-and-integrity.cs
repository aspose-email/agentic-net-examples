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
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file into a MapiMessage instance.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Check whether the message is signed.
                if (!msg.IsSigned)
                {
                    Console.WriteLine("The message is not signed.");
                    return;
                }

                try
                {
                    // Validate the digital signature and retrieve signer certificates.
                    X509Certificate2[] signerCertificates = msg.CheckSignature();

                    Console.WriteLine($"Signature is valid. Number of signers: {signerCertificates.Length}");
                    foreach (X509Certificate2 cert in signerCertificates)
                    {
                        Console.WriteLine($"Signer Subject: {cert.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle signature validation errors (e.g., invalid signature, unsupported type).
                    Console.Error.WriteLine($"Signature validation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
