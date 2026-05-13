using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for certificate and output folder
            string certPath = "publicCert.cer";
            string outputDir = "EncryptedMessages";

            // Verify certificate file exists
            if (!File.Exists(certPath))
            {
                Console.Error.WriteLine($"Certificate file not found: {certPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the X509 certificate
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

            // Prepare a batch of mail messages
            List<MailMessage> messages = new List<MailMessage>();
            for (int i = 1; i <= 10; i++)
            {
                MailMessage msg = new MailMessage();
                msg.From = "sender@example.com";
                msg.To = "recipient@example.com";
                msg.Subject = $"Test message {i}";
                msg.Body = $"This is the body of message {i}.";
                messages.Add(msg);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int index = 0;
            foreach (MailMessage message in messages)
            {
                // Encrypt the message
                MailMessage encryptedMessage = message.Encrypt(publicCert);

                // Save encrypted message to file
                string filePath = Path.Combine(outputDir, $"encrypted_{index}.eml");
                try
                {
                    encryptedMessage.Save(filePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save encrypted message {index}: {ex.Message}");
                }

                // Dispose encrypted message
                encryptedMessage.Dispose();

                index++;
            }

            // Dispose original messages
            foreach (MailMessage original in messages)
            {
                original.Dispose();
            }

            stopwatch.Stop();
            Console.WriteLine($"Encrypted {messages.Count} messages in {stopwatch.ElapsedMilliseconds} ms.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
