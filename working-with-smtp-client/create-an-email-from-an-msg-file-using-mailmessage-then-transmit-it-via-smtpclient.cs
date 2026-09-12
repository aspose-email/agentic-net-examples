using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source MSG file
            const string msgPath = "input.msg";

            // Verify the MSG file exists before attempting to load it
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

                Console.Error.WriteLine($"Error: MSG file not found at '{msgPath}'.");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapMsg = MapiMessage.Load(msgPath);

            // Convert the MapiMessage to a MailMessage for sending
            MailMessage mailMessage = mapMsg.ToMailMessage(new MailConversionOptions());

            // SMTP server configuration (replace with real values)
            const string smtpHost = "smtp.example.com";
            const int smtpPort = 587;
            const string smtpUser = "username@example.com";
            const string smtpPassword = "password";

            // Send the email using SmtpClient
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = smtpHost;
                smtpClient.Port = smtpPort;
                smtpClient.SecurityOptions = SecurityOptions.Auto;
                smtpClient.Username = smtpUser;
                smtpClient.Password = smtpPassword;

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
