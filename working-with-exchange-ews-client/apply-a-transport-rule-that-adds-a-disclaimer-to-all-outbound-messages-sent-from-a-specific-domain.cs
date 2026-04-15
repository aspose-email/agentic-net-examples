using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a mail message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@domain.com");
                message.Subject = "Test Email with Disclaimer";

                // Original body
                string originalBody = "Hello,\nThis is a test email.";
                // Disclaimer to be appended
                string disclaimer = "\n---\nThis email contains a disclaimer.";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Combine body and disclaimer
                message.Body = originalBody + disclaimer;

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent successfully with disclaimer.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
