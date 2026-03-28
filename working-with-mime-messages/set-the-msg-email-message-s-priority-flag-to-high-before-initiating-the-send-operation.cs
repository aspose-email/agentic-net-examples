using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Create a mail message and set its priority to High
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test Body"))
            {
                message.Priority = MailPriority.High;

                // Initialize the SMTP client (preserve the variable name 'client')
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending message: {ex.Message}");
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
