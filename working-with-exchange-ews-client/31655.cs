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
            // Define the EWS endpoint and credentials for the shared mailbox
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string sharedMailbox = "sharedmailbox@example.com";
            string password = "your_password";

            NetworkCredential credentials = new NetworkCredential(sharedMailbox, password);

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new mail message
                MailMessage message = new MailMessage();
                message.From = sharedMailbox;
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email with Disclaimer";
                message.Body = "Hello,\nThis is the email body.";

                // Append a custom disclaimer to the footer
                message.Body += "\n\n---\nThis email is confidential and intended only for the recipient.";

                // Send the message
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
