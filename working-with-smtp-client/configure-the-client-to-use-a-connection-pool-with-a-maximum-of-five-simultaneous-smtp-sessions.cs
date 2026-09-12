using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

public class Program
{
    public static void Main()
    {
        try
        {
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network calls when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping connection.");
                return;
            }

            using (SmtpClient smtpClient = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Configure connection pool with a maximum of five simultaneous sessions
                smtpClient.ConnectionsQuantity = 5;

                using (MailMessage message = new MailMessage("from@example.com", "to@example.com", "Test Subject", "Hello World"))
                {
                    try
                    {
                        smtpClient.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (SmtpException ex)
                    {
                        Console.Error.WriteLine($"SMTP error: {ex.Message}");
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
