using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        // Author note: simple console app to load an MSG file and send it via SMTP.
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load MSG as MapiMessage
            MapiMessage mapiMessage = MapiMessage.Load(msgPath);

            // Convert to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions);

            // Send using SmtpClient
            using (mailMessage)
            using (SmtpClient smtpClient = new SmtpClient())
            {
                // Configure SMTP client (replace with real server details)
                smtpClient.Host = "smtp.example.com";
                smtpClient.Port = 587;
                smtpClient.SecurityOptions = SecurityOptions.Auto;
                smtpClient.Username = "user@example.com";
                smtpClient.Password = "password";

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
