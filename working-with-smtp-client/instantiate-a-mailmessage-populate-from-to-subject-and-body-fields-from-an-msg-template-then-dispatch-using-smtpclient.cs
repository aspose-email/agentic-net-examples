using System;
using System.IO;
using System.Linq;
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
            // Path to the MSG template
            const string msgPath = "template.msg";

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

                Console.Error.WriteLine($"MSG template not found at path: {msgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapiMessage = MapiMessage.Load(msgPath);

            // Convert MapiMessage to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
            {
                // Prepare SMTP client
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    try
                    {
                        // Configure SMTP server (replace with real values)
                        smtpClient.Host = "smtp.example.com";
                        smtpClient.Port = 587;
                        smtpClient.SecurityOptions = SecurityOptions.Auto;
                        smtpClient.Username = "user@example.com";
                        smtpClient.Password = "password";

                        // Build recipient list string
                        string recipients = string.Join(";", mailMessage.To.Select(a => a.Address));

                        // Send the email using the fields from the template
                        smtpClient.Send(mailMessage.From.Address, recipients, mailMessage.Subject, mailMessage.Body);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                        return;
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
