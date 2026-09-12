using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Create the e‑mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "organizer@example.com";
                message.To.Add("attendee@example.com");
                message.Subject = "Meeting Request";
                message.Body = "Please attend the meeting.";
                // Mark the meeting request as high‑importance
                message.Priority = MailPriority.High;

                // Placeholder connection details
                string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                    return;
                }

                // Send the message via Exchange WebDav client
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending message: {ex.Message}");
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
