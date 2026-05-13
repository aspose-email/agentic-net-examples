using Aspose.Email.Clients;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define the folder containing the EML files.
            string emlFolder = "Emails";
            if (!Directory.Exists(emlFolder))
            {
                Console.Error.WriteLine($"Folder not found: {emlFolder}");
                return;
            }

            // Gather all EML files.
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(emlFolder, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            if (emlFiles.Length == 0)
            {
                Console.Error.WriteLine("No EML files found to process.");
                return;
            }

            // Prepare a list to hold the modified messages.
            List<MailMessage> messagesToSend = new List<MailMessage>();

            foreach (string emlPath in emlFiles)
            {
                if (!File.Exists(emlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {emlPath}");
                    continue;
                }

                try
                {
                    // Load the original EML message.
                    using (MailMessage original = MailMessage.Load(emlPath))
                    {
                        // Convert to MAPI message to set the read receipt flag.
                        using (MapiMessage mapi = MapiMessage.FromMailMessage(original))
                        {
                            mapi.ReadReceiptRequested = true;

                            // Convert back to MailMessage for sending.
                            MailMessage updated = mapi.ToMailMessage(new MailConversionOptions());

                            // Add to the send list.
                            messagesToSend.Add(updated);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{emlPath}': {ex.Message}");
                }
            }

            if (messagesToSend.Count == 0)
            {
                Console.Error.WriteLine("No messages prepared for sending.");
                return;
            }

            // Placeholder SMTP configuration.
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Guard against placeholder credentials/hosts.
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping actual send.");
                return;
            }

            // Send the batch of messages.
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    foreach (MailMessage msg in messagesToSend)
                    {
                        try
                        {
                            client.Send(msg);
                            Console.WriteLine($"Sent: {msg.Subject}");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Failed to send '{msg.Subject}': {sendEx.Message}");
                        }
                        finally
                        {
                            msg.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP client error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
