using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Enter your country: ");
            string country = Console.ReadLine();

            Console.Write("Enter your mobile number: ");
            string mobileNumber = Console.ReadLine();

            // Generate a 6‑digit verification code
            Random random = new Random();
            int verificationCode = random.Next(100000, 1000000);

            // Prepare the email message
            MailMessage message = new MailMessage();
            message.From = "no-reply@example.com";
            message.To = "user@example.com"; // Replace with recipient address
            message.Subject = "Your Verification Code";
            message.Body = $"Country: {country}\nMobile: {mobileNumber}\nVerification Code: {verificationCode}";

            // SMTP server configuration (replace placeholders with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "smtp_user";
            string smtpPassword = "smtp_password";

            // Send the email using SmtpClient
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword, SecurityOptions.Auto))
                {
                    client.Send(message);
                }

                Console.WriteLine("Verification code sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}