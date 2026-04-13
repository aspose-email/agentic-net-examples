using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder Exchange server details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping email operation.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Compose the email message
                using (MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Subject: Confidential Information",
                    "This email contains confidential information. Please handle with care."))
                {
                    // If the body contains the word "confidential", request a delivery receipt
                    if (message.Body != null &&
                        message.Body.IndexOf("confidential", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    }

                    // Send the message via Exchange
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
