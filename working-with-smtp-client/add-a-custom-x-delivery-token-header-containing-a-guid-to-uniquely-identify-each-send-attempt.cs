using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        // Placeholder SMTP settings – replace with real values to enable sending.
        string host = "smtp.example.com";
        int port = 587;
        string username = "YOUR_USERNAME";
        string password = "YOUR_PASSWORD";

        if (host == "smtp.example.com")
        {
            Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
            return;
        }

        using (MailMessage message = new MailMessage())
        {
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test email with custom header";
            message.Body = "This email includes a unique X-Delivery-Token header.";

            // Add a custom X-Delivery-Token header containing a GUID.
            string deliveryToken = Guid.NewGuid().ToString();
            message.Headers.Add("X-Delivery-Token", deliveryToken);

            try
            {
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
