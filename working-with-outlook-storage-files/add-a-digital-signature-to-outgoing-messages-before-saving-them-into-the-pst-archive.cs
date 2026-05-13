using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the certificate and PST file
            const string certificatePath = "certificate.pfx";
            const string certificatePassword = "password";
            const string pstPath = "archive.pst";

            // Verify certificate file exists
            if (!File.Exists(certificatePath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certificatePath}");
                return;
            }

            // Load the X509 certificate
            X509Certificate2 certificate;
            try
            {
                certificate = new X509Certificate2(certificatePath, certificatePassword);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                return;
            }

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Prepare a simple mail message
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Signed Message",
                    "This is a digitally signed message.");

                // Sign the message using SecureEmailManager
                SecureEmailManager secureManager = new SecureEmailManager();
                MailMessage signedMessage;
                try
                {
                    signedMessage = secureManager.AttachSignature(message, certificate);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach signature: {ex.Message}");
                    return;
                }

                // Convert the signed MailMessage to a MapiMessage
                MapiMessage mapiMessage;
                try
                {
                    mapiMessage = MapiMessage.FromMailMessage(signedMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert to MapiMessage: {ex.Message}");
                    return;
                }

                // Add the signed message to the PST root folder
                try
                {
                    string entryId = pst.RootFolder.AddMessage(mapiMessage);
                    Console.WriteLine($"Signed message added to PST. EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
