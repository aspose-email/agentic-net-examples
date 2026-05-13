using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Prepare a collection of recipients
            MailAddressCollection toRecipients = new MailAddressCollection();
            toRecipients.Add(new MailAddress("recipient1@example.com"));
            toRecipients.Add(new MailAddress("recipient2@example.com"));
            toRecipients.Add(new MailAddress("recipient3@example.com"));

            // Initialize the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(username);
                    message.Subject = "Test Email with Multiple To Recipients";
                    message.Body = "This email demonstrates adding multiple To recipients.";

                    // Add each recipient to the To collection
                    foreach (MailAddress address in toRecipients)
                    {
                        message.To.Add(address);
                    }

                    // Send the message
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
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
