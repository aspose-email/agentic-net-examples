using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file containing the email and SMTP configuration
            string msgFilePath = "email.msg";

            // Verify the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapMsg;
            try
            {
                mapMsg = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Convert the MapiMessage to a MailMessage for sending
            MailMessage mailMessage;
            try
            {
                mailMessage = mapMsg.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert MSG to MailMessage: {ex.Message}");
                return;
            }

            // Extract SMTP configuration from custom headers (if present)
            string smtpHost = mailMessage.Headers["X-Smtp-Host"];
            string smtpPortString = mailMessage.Headers["X-Smtp-Port"];
            string smtpUser = mailMessage.Headers["X-Smtp-User"];
            string smtpPassword = mailMessage.Headers["X-Smtp-Password"];

            // Basic validation of required SMTP settings
            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPortString))
            {
                Console.Error.WriteLine("SMTP host or port not specified in the message headers.");
                return;
            }

            int smtpPort;
            if (!int.TryParse(smtpPortString, out smtpPort))
            {
                Console.Error.WriteLine($"Invalid SMTP port value: {smtpPortString}");
                return;
            }

            // Create and configure the SmtpClient
            using (SmtpClient smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;

                    // If credentials are provided, set them
                    if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPassword))
                    {
                        smtpClient.Username = smtpUser;
                        smtpClient.Password = smtpPassword;
                    }

                    // Use automatic security option; adjust if needed
                    smtpClient.SecurityOptions = SecurityOptions.Auto;

                    // Send the email
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (SmtpException smtpEx)
                {
                    Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
