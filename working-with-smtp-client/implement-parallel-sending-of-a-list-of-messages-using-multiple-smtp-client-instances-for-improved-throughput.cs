using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials/host
            if (smtpHost.Contains("example.com") || smtpUsername.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Prepare a list of messages to send
            List<MailMessage> messages = new List<MailMessage>();
            for (int i = 1; i <= 10; i++)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(smtpUsername);
                msg.To.Add(new MailAddress($"recipient{i}@example.com"));
                msg.Subject = $"Test Message {i}";
                msg.Body = $"This is the body of test message {i}.";
                messages.Add(msg);
            }

            // Determine degree of parallelism
            int maxDegree = Environment.ProcessorCount;

            // Parallel sending using thread‑local SmtpClient instances
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegree };
            Parallel.ForEach(
                messages,
                parallelOptions,
                () =>
                {
                    // Initialize a client for this thread
                    SmtpClient client = null;
                    try
                    {
                        client = new SmtpClient(smtpHost, smtpPort, SecurityOptions.None);
                        client.Username = smtpUsername;
                        client.Password = smtpPassword;
                        client.ValidateCredentials();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to initialize SMTP client: {ex.Message}");
                    }
                    return client;
                },
                (msg, state, localClient) =>
                {
                    if (localClient != null)
                    {
                        try
                        {
                            localClient.Send(msg);
                            Console.WriteLine($"Sent message to {msg.To[0].Address}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send message to {msg.To[0].Address}: {ex.Message}");
                        }
                    }
                    return localClient;
                },
                (localClient) =>
                {
                    // Dispose the client for this thread
                    if (localClient != null)
                    {
                        localClient.Dispose();
                    }
                });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
