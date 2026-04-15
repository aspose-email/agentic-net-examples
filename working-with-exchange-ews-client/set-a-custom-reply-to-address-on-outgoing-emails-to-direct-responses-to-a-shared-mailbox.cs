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
            // Exchange server connection details
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Desired Reply-To address (shared mailbox)
            string sharedReplyTo = "sharedmailbox@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || sharedReplyTo.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("user@example.com");
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Test email with custom Reply-To";
            message.Body = "This email uses a custom Reply-To address.";
            // Set custom Reply-To header
            message.Headers.Add("Reply-To", sharedReplyTo);

            // Initialize EWS client and send the message
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    client.Send(message);
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
