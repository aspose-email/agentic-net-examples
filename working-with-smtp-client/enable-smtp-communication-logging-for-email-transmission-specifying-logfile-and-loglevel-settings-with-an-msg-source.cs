using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;

namespace SmtpLoggingExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the source MSG file
                string msgPath = "sample.msg";

                // Verify that the MSG file exists before attempting to load it
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

                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the MSG file as a MapiMessage
                MapiMessage mapiMessage = MapiMessage.Load(msgPath);

                // Convert the MapiMessage to a MailMessage for SMTP transmission
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Configure the SMTP client
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = "smtp.example.com";
                        smtpClient.Port = 587;
                        smtpClient.Username = "user@example.com";
                        smtpClient.Password = "password";

                        // Enable communication logging
                        smtpClient.EnableLogger = true;
                        smtpClient.LogFileName = "smtp.log";

                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
