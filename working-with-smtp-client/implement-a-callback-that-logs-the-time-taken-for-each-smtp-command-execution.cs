using Aspose.Email.Clients;
using System;
using System.Diagnostics;
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

            // Guard against placeholder credentials/hosts
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping execution.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Enable built‑in logger (optional)
                client.EnableLogger = true;
                client.LogFileName = "smtp_commands.log";

                // Prepare a simple email message
                MailMessage message = new MailMessage(
                    from: username,
                    to: "recipient@example.com",
                    subject: "Test Email",
                    body: "This is a test message."
                );

                // Send the message while measuring the time taken for the Send command
                ExecuteWithTiming(() => client.Send(message), "Send");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Executes an SMTP command delegate and logs its execution time
    private static void ExecuteWithTiming(Action smtpCommand, string commandName)
    {
        Stopwatch stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            smtpCommand();
            stopwatch.Stop();
            Console.WriteLine($"{commandName} command completed in {stopwatch.Elapsed.TotalMilliseconds} ms.");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            Console.Error.WriteLine($"{commandName} command failed after {stopwatch.Elapsed.TotalMilliseconds} ms. Error: {ex.Message}");
            throw;
        }
    }
}
