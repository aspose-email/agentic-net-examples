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
            // Mailbox connection details
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "legaldept@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare the email message
                MailMessage message = new MailMessage();
                message.From = "legaldept@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Legal Department Confidential Email";
                message.Body = "Please find the confidential information attached.";

                // Set the classification header to 'Confidential'
                message.Headers.Add("X-MS-Exchange-Organization-Classification", "Confidential");

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent with classification header.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
