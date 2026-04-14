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
            // Define mailbox URI and credentials for the shared service mailbox
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("service_user", "service_password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare the email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("service@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Subject";
                message.Body = "This is a sample email body.";

                // Set a custom Reply-To address
                message.ReplyToList.Add(new MailAddress("customreply@example.com"));

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
