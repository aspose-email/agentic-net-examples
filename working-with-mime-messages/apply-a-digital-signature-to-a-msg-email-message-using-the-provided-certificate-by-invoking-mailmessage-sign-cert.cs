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
            string inputMsgPath = "input.msg";
            string certPath = "certificate.pfx";
            string outputMsgPath = "signed.msg";

            // Ensure input MSG exists; create minimal placeholder if missing
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

                using (MapiMessage placeholder = new MapiMessage("placeholder@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                {
                    placeholder.Save(inputMsgPath);
                }
                Console.Error.WriteLine($"Input MSG not found. Created placeholder at '{inputMsgPath}'.");
                return;
            }

            // Ensure certificate file exists
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file '{certPath}' not found.");
                return;
            }

            // Load certificate (placeholder password)
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certPath, "password");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load MSG, convert to MailMessage, attach signature, and save
            using (MapiMessage mapimsg = MapiMessage.Load(inputMsgPath))
            {
                using (MailMessage mail = mapimsg.ToMailMessage(new MailConversionOptions()))
                {
                    MailMessage signed = mail.AttachSignature(certificate);
                    using (signed)
                    {
                        signed.Save(outputMsgPath);
                    }
                }
            }

            Console.WriteLine($"Signed message saved to '{outputMsgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
