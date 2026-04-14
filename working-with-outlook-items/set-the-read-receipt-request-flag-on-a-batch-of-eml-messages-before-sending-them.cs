using Aspose.Email.Clients;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailReadReceiptBatch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP configuration
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUsername = "user@example.com";
                string smtpPassword = "password";

                // Guard against placeholder credentials/host
                if (smtpHost.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                    return;
                }

                // Directory containing EML files
                string emlFolderPath = "Emails";

                // Verify folder existence
                if (!Directory.Exists(emlFolderPath))
                {
                    Console.Error.WriteLine($"Folder does not exist: {emlFolderPath}");
                    return;
                }

                // Get list of EML files
                string[] emlFilePaths = Directory.GetFiles(emlFolderPath, "*.eml");
                if (emlFilePaths.Length == 0)
                {
                    Console.Error.WriteLine("No EML files found in the specified folder.");
                    return;
                }

                // Prepare SMTP client
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;
                    smtpClient.Username = smtpUsername;
                    smtpClient.Password = smtpPassword;
                    smtpClient.SecurityOptions = SecurityOptions.Auto;

                    // Validate credentials safely
                    try
                    {
                        smtpClient.ValidateCredentials();
                    }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"SMTP credential validation failed: {credEx.Message}");
                        return;
                    }

                    // Load messages, set read receipt flag, and collect them
                    List<MailMessage> messagesToSend = new List<MailMessage>();
                    foreach (string emlPath in emlFilePaths)
                    {
                        try
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

                                Console.Error.WriteLine($"EML file not found: {emlPath}");
                                continue;
                            }

                            MailMessage mailMessage = MailMessage.Load(emlPath);
                            // Request read receipt by setting the ReadReceiptTo address
                            mailMessage.ReadReceiptTo = "readreceipt@example.com";

                            messagesToSend.Add(mailMessage);
                        }
                        catch (Exception loadEx)
                        {
                            Console.Error.WriteLine($"Failed to load or modify '{emlPath}': {loadEx.Message}");
                        }
                    }

                    if (messagesToSend.Count == 0)
                    {
                        Console.Error.WriteLine("No valid messages to send after processing.");
                        return;
                    }

                    // Send all messages in a batch
                    try
                    {
                        smtpClient.Send(messagesToSend.ToArray());
                        Console.WriteLine("All messages sent successfully.");
                    }
                    catch (Exception sendEx)
                    {
                        Console.Error.WriteLine($"Error sending messages: {sendEx.Message}");
                    }
                    finally
                    {
                        // Dispose each MailMessage
                        foreach (MailMessage msg in messagesToSend)
                        {
                            msg.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
