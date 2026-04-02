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

            // Guard file existence
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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load MSG file
            using (MapiMessage mapimsg = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage with required options
                using (MailMessage mail = mapimsg.ToMailMessage(new MailConversionOptions()))
                {
                    if (mail.IsSigned)
                    {
                        // Verify signature and retrieve signer certificates
                        X509Certificate2[] signers = mail.CheckSignature();
                        Console.WriteLine($"Signature is valid. Number of signers: {signers.Length}");
                        foreach (var cert in signers)
                        {
                            Console.WriteLine($"Signer: {cert.Subject}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The message is not signed.");
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
