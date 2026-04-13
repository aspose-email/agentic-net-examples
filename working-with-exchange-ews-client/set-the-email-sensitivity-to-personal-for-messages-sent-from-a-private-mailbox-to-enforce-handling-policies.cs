using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Mailbox connection details (replace with actual values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and configure the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new mail message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Policy Enforced Email";
                message.Body = "This email is marked as Personal sensitivity.";
                
                // Set the sensitivity to Personal
                message.Sensitivity = MailSensitivity.Personal;

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent with Personal sensitivity.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
