using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder SMTP configuration
                string host = "smtp.example.com";
                int port = 587;
                string username = "username";
                string password = "password";

                // Path to the MSG file
                string msgPath = "message.msg";

                // Skip sending when placeholder configuration is detected
                if (host == "smtp.example.com" || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                    return;
                }

                // Verify that the MSG file exists
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
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

                // Load the message from the MSG file
                MailMessage message;
                try
                {
                    message = MailMessage.Load(msgPath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
                    return;
                }

                using (message)
                {
                    // Create and configure the SMTP client
                    using (SmtpClient client = new SmtpClient(host, port, username, password))
                    {
                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Message sent successfully.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Failed to send message: {sendEx.Message}");
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
