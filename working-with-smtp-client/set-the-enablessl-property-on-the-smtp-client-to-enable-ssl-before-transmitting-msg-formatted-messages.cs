using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

namespace SmtpEnableSslExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the path to the MSG file.
            const string msgFilePath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    MailMessage placeholder = new MailMessage
                    {
                        From = "placeholder@example.com",
                        To = "recipient@example.com",
                        Subject = "Placeholder Message",
                        Body = "This is a placeholder MSG file."
                    };
                    placeholder.Save(msgFilePath);
                    Console.WriteLine($"Created placeholder MSG file at '{msgFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage.
            MapiMessage mapMsg;
            try
            {
                mapMsg = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Convert the MapiMessage to a MailMessage for sending.
            MailMessage mailMessage;
            try
            {
                mailMessage = mapMsg.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting MSG to MailMessage: {ex.Message}");
                return;
            }

            // Configure the SMTP client.
            // Placeholder credentials are used; real credentials should be supplied for actual sending.
            const string smtpHost = "smtp.example.com";
            const int smtpPort = 587;
            const string smtpUser = "user@example.com";
            const string smtpPass = "password";

            // Guard against sending with placeholder credentials.
            if (smtpHost.Contains("example.com") || smtpUser.Contains("example.com"))
            {
                Console.WriteLine("SMTP client is configured with placeholder credentials. Skipping send operation.");
                return;
            }

            SmtpClient client = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                Username = smtpUser,
                Password = smtpPass,
                // Enable SSL before transmitting the message.
                SecurityOptions = SecurityOptions.SSLExplicit
            };

            // Send the message inside a try/catch block.
            try
            {
                client.Send(mailMessage);
                Console.WriteLine("Message sent successfully via SMTP with SSL enabled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message: {ex.Message}");
            }
        }
    }
}
