using System;
using System.Net.NetworkInformation;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping email operation.");
                return;
            }

            // Measure network latency to the SMTP host
            Ping ping = new Ping();
            PingReply reply = ping.Send(host);
            int latencyMs = (reply.Status == IPStatus.Success) ? (int)reply.RoundtripTime : 1000;

            // Calculate an appropriate timeout (base 5 seconds + twice the latency)
            int calculatedTimeout = 5000 + latencyMs * 2;

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                client.Timeout = calculatedTimeout;

                // Prepare a simple test email
                using (MailMessage message = new MailMessage())
                {
                    message.From = username;
                    message.To.Add(username);
                    message.Subject = "Timeout Adjustment Test";
                    message.Body = "This email demonstrates dynamic timeout configuration.";

                    try
                    {
                        client.Send(message);
                        Console.WriteLine($"Email sent successfully. Timeout used: {client.Timeout} ms.");
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
