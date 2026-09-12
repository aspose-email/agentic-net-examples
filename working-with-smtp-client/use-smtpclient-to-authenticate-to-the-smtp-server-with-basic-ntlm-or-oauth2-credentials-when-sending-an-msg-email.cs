using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSmtpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file to be sent
                string msgPath = "sample.msg";

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

                    Console.Error.WriteLine($"Input MSG file not found: {msgPath}");
                    return;
                }

                // Load the MSG file as a MAPI message
                MapiMessage mapMsg = MapiMessage.Load(msgPath);

                // Convert the MAPI message to a MailMessage
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapMsg.ToMailMessage(conversionOptions))
                {
                    // SMTP server configuration (replace with actual values)
                    string smtpHost = "smtp.example.com";
                    int smtpPort = 587;
                    string smtpUser = "user@example.com";
                    string smtpPassword = "password"; // For Basic authentication
                    // For OAuth2 authentication, set useOAuth to true and provide the access token instead of password
                    // string oauthAccessToken = "ya29.a0AfH6SM...";

                    // Create and configure the SmtpClient (Basic authentication with explicit security options)
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword, false, SecurityOptions.Auto))
                    {
                        try
                        {
                            // Send the email
                            client.Send(mailMessage);
                            Console.WriteLine("Message sent successfully.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Error sending email: {sendEx.Message}");
                        }
                    }

                    // Example for OAuth2 authentication (uncomment to use)
                    /*
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpUser, oauthAccessToken, true))
                    {
                        try
                        {
                            client.Send(mailMessage);
                            Console.WriteLine("Message sent successfully with OAuth2.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Error sending email with OAuth2: {sendEx.Message}");
                        }
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
