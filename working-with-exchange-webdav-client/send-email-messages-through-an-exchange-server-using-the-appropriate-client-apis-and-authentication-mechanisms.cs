using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace ExchangeEmailSender
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange server connection details (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Initialize the Exchange client
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Optional: enable pre-authentication
                    client.PreAuthenticate = true;

                    // Create the email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "user@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email via Exchange";
                        message.Body = "Hello, this is a test email sent using Aspose.Email ExchangeClient.";

                        // Send the message
                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Message sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error sending email: {ex.Message}");
                            return;
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
}
