using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgPath = "email.msg";

                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                Aspose.Email.MailMessage message;
                try
                {
                    message = Aspose.Email.MailMessage.Load(msgPath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
                    return;
                }

                using (message)
                {
                    string host = "smtp.example.com";
                    int port = 587;
                    string username = "user@example.com";
                    string password = "password";

                    try
                    {
                        using (Aspose.Email.Clients.Smtp.SmtpClient client = new Aspose.Email.Clients.Smtp.SmtpClient(host, port, username, password, Aspose.Email.Clients.SecurityOptions.Auto))
                        {
                            client.Send(message);
                            Console.WriteLine("Message sent successfully.");
                        }
                    }
                    catch (Exception smtpEx)
                    {
                        Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
                        return;
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