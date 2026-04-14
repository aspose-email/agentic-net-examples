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
            // EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                return;
            }

            if (client == null)
            {
                Console.Error.WriteLine("EWS client is null.");
                return;
            }

            using (client)
            {
                // Compose the email
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Test Subject",
                    "This is a test message with read receipt and delivery notifications.");

                // Request a read receipt
                message.ReadReceiptTo = "sender@example.com";

                // Request delivery notifications (on success and on failure)
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess |
                                                       DeliveryNotificationOptions.OnFailure;

                // Send the message
                try
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to send email: " + ex.Message);
                    return;
                }

                // Log the requested callbacks
                Console.WriteLine("Read receipt requested to: " + message.ReadReceiptTo);
                Console.WriteLine("Delivery notifications requested: " + message.DeliveryNotificationOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
