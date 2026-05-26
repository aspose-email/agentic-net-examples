using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.DeliveryService.SendGrid;

class Program
{
    static void Main()
    {
        try
        {
            // Define environment (development, staging, production)
            string environment = "development";

            // Create the email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email with X-Environment Header";
            message.Body = "This email includes a custom X-Environment header.";

            // Add custom X-Environment header
            message.Headers.Add("X-Environment", environment);

            // Placeholder SendGrid API key
            string apiKey = "YOUR_API_KEY";

            // Guard against placeholder credentials
            if (apiKey == "YOUR_API_KEY")
            {
                Console.Error.WriteLine("SendGrid API key is a placeholder. Skipping send operation.");
                return;
            }

            // Create SendGrid client and send the message
            using (SendGridClient client = new SendGridClient(apiKey))
            {
                try
                {
                    // The Send method expects a list of categories; an empty list is acceptable
                    List<string> categories = new List<string>();
                    client.Send(message, categories, null);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
