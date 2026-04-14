using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS client configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare a batch of personalized messages
                List<MailMessage> messages = new List<MailMessage>();
                for (int i = 1; i <= 20; i++)
                {
                    MailMessage message = new MailMessage();
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = $"Personalized Subject #{i}";
                    message.Body = $"Hello,\n\nThis is a personalized email number {i}.\nBest regards.";
                    messages.Add(message);
                }

                // Send messages concurrently using Task.Run (EWS async API is prohibited)
                List<Task> sendTasks = new List<Task>();
                foreach (MailMessage msg in messages)
                {
                    sendTasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            client.Send(msg);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send message '{msg.Subject}': {ex.Message}");
                        }
                    }));
                }

                // Wait for all send operations to complete
                Task.WaitAll(sendTasks.ToArray());
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
