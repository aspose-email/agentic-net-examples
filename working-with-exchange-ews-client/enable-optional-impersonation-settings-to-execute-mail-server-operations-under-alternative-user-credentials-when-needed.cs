using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Author: Sample demonstrating optional impersonation with EWS client
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "admin@example.com";
            string password = "adminPassword";

            // Create EWS client
            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);
            try
            {
                // Optional impersonation: act as another user
                // Impersonate using the primary SMTP address of the target user
                client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                // Build a simple email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("impersonated@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Impersonated Email via EWS";
                message.Body = "This email was sent using impersonation.";

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent successfully under impersonated identity.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
            finally
            {
                // Ensure the client is properly disposed
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
