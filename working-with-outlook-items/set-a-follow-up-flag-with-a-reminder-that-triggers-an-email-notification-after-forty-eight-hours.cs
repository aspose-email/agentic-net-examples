using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create a simple mail message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Follow‑up Required";
            message.Body = "Please review the attached document.";

            // Configure follow‑up options with a reminder after 48 hours
            DateTime startDate = DateTime.Now;
            DateTime dueDate = startDate.AddHours(48);
            FollowUpOptions options = new FollowUpOptions("Follow up", startDate, dueDate);
            options.ReminderTime = dueDate; // Reminder at due date (48 hours later)

            // Send the message with follow‑up options using EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                try
                {
                    client.Send(message, options);
                    Console.WriteLine("Message sent with follow‑up flag and 48‑hour reminder.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
