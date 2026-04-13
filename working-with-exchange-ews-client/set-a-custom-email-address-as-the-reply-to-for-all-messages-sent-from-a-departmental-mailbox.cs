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
            // EWS client configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "department_user";
            string password = "password";
            string customReplyTo = "custom-reply@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || password == "password" || customReplyTo.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Compose a new mail message
                    MailMessage message = new MailMessage();
                    message.From = "department@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Message";
                    message.Body = "This is a test email sent from the departmental mailbox.";

                    // Set custom Reply-To address via header
                    message.Headers.Add("Reply-To", customReplyTo);

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully with custom Reply-To.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
