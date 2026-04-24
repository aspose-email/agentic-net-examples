using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Skip actual sending when placeholder values are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test email with delivery notification";
                    message.Body = "This email requests a delivery failure notification.";

                    // Set delivery notification options to receive alerts on failure
                    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    // Send the message
                    try
                    {
                        smtpClient.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception sendEx)
                    {
                        Console.Error.WriteLine($"Error sending message: {sendEx.Message}");
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
