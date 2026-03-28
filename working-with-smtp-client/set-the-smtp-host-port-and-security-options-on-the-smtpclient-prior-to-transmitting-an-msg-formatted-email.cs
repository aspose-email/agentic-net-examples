using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(msgPath))
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = "smtp.example.com";
                    client.Port = 587;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Username = "user@example.com";
                    client.Password = "password";

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
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
