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
            // Define EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Add a custom header to all outgoing requests
                client.AddHeader("X-Custom-Header", "MyHeaderValue");

                // Create a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(username);
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = "Test Email with Custom Header";
                    message.Body = "This email was sent using Aspose.Email with a custom header.";

                    // Send the message
                    client.Send(message);
                }

                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
