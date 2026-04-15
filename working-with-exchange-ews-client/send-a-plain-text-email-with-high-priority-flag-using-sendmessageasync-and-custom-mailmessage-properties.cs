using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main()
    {
        try
        {
            // Mailbox URI and credentials for the Exchange server
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Obtain an asynchronous EWS client
            IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            using (client)
            {
                // Create a plain‑text mail message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "High Priority Test";
                message.Body = "This is a plain‑text email marked as high priority.";
                message.Priority = MailPriority.High;

                // Add a custom header
                message.Headers.Add("X-Custom-Header", "CustomValue");

                // Send the message asynchronously
                await client.SendAsync(message);
                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
