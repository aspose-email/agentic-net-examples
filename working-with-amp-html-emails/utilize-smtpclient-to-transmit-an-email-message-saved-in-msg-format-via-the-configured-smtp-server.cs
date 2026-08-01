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
            // Define paths
            string msgFilePath = "message.msg";

            // Verify input MSG file exists
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

            // Convert MapiMessage to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMessage;
            try
            {
                mailMessage = mapMsg.ToMailMessage(conversionOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion to MailMessage failed: {ex.Message}");
                return;
            }

            // Configure SMTP client (replace placeholders with real values)
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.example.com";
            client.Port = 587;
            client.Username = "user@example.com";
            client.Password = "password";
            client.SecurityOptions = SecurityOptions.Auto;

            // Send the email
            try
            {
                client.Send(mailMessage);
                Console.WriteLine("Message sent successfully.");
            }
            catch (SmtpException smtpEx)
            {
                Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send message: {ex.Message}");
            }
            finally
            {
                // Dispose resources
                client.Dispose();
                mailMessage.Dispose();
                mapMsg.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
