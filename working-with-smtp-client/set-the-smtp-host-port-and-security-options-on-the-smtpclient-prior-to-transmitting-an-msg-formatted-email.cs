using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

namespace AsposeEmailSmtpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define SMTP server settings
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUser = "user@example.com";
                string smtpPass = "password";
                SecurityOptions smtpSecurity = SecurityOptions.Auto; // Adjust as needed (e.g., SSLImplicit)

                // Path to the MSG file to be sent
                string msgPath = "sample.msg";

                // Verify the MSG file exists
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

                    Console.Error.WriteLine($"Input file not found: {msgPath}");
                    return;
                }

                // Load the MSG file into a MapiMessage
                MapiMessage mapMsg = MapiMessage.Load(msgPath);

                // Convert MapiMessage to MailMessage
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapMsg.ToMailMessage(conversionOptions))
                {
                    // Initialize the SMTP client with host, port, credentials, and security options
                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, smtpSecurity))
                    {
                        try
                        {
                            // Send the email
                            smtpClient.Send(mailMessage);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
