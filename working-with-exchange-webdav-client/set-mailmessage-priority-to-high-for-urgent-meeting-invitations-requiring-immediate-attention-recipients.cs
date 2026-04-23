using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (placeholders)
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange URL detected. Skipping network operation.");
                return;
            }

            // Create the mail message
            MailMessage message = new MailMessage();
            message.From = "organizer@example.com";
            message.To.Add("attendee1@example.com");
            message.To.Add("attendee2@example.com");
            message.Subject = "Urgent Meeting Invitation";
            message.Body = "Please attend the meeting at 10:00 AM today.";
            // Set high priority
            message.Priority = MailPriority.High;

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent with high priority.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
