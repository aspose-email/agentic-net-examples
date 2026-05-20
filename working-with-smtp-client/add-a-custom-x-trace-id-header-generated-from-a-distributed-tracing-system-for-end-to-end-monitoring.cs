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
            // Placeholder values – replace with real credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Create the EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Create a new mail message.
                MailMessage message = null;
                try
                {
                    message = new MailMessage();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create MailMessage: {ex.Message}");
                    return;
                }

                using (message)
                {
                    // Set basic properties.
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email with X-Trace-Id Header";
                    message.Body = "This email includes a custom X-Trace-Id header for tracing.";

                    // Generate a trace identifier (simulated distributed tracing system).
                    string traceId = Guid.NewGuid().ToString();

                    // Add the custom header.
                    message.Headers.Add("X-Trace-Id", traceId);

                    // Send the message.
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully with X-Trace-Id: " + traceId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
