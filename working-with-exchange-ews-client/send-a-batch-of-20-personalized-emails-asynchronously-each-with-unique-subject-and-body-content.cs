using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Create EWS client.
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client as IDisposable)
            {
                // Prepare 20 personalized messages.
                var messages = new List<MailMessage>();
                for (int i = 1; i <= 20; i++)
                {
                    var msg = new MailMessage
                    {
                        From = "sender@example.com",
                        Subject = $"Personalized Subject #{i}",
                        Body = $"Hello,\n\nThis is personalized email number {i}.\nBest regards."
                    };
                    msg.To.Add("recipient@example.com");
                    messages.Add(msg);
                }

                // Send messages asynchronously, one by one.
                try
                {
                    foreach (var msg in messages)
                    {
                        await Task.Run(() => client.Send(msg));
                    }
                    Console.WriteLine("All messages sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
