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
            // Initialize the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(
                    "https://exchange.example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Create a new mail message with custom metadata
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test with custom metadata";
                message.Body = "This email contains custom metadata properties.";

                // Add custom metadata as headers
                message.Headers.Add("X-Custom-Property1", "Value1");
                message.Headers.Add("X-Custom-Property2", "Value2");

                // Send the message
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }

            // Dispose the client if it implements IDisposable
            if (client is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
