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
            // Placeholder credentials – replace with real values or skip network call.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Detect placeholder credentials and skip sending.
            if (username == "username" && password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create the mail message and set its sensitivity to Confidential.
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To = "recipient@example.com";
            message.Subject = "Confidential Information";
            message.Body = "Please handle this email with care.";
            message.Sensitivity = MailSensitivity.CompanyConfidential;

            // Connect to Exchange using EWS and send the message.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully with Confidential sensitivity.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
