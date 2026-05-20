using Aspose.Email.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Detect placeholder credentials and skip actual sending
            if (smtpHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping email send.");
                return;
            }

            // Prepare cancellation support (e.g., cancel after 10 seconds)
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Optional: cancel automatically after a timeout
                cts.CancelAfter(TimeSpan.FromSeconds(10));
                CancellationToken token = cts.Token;

                // Create the email message
                using (MailMessage message = new MailMessage(
                    "from@example.com",
                    "to@example.com",
                    "Async Email with Cancellation",
                    "This email was sent using Aspose.Email's asynchronous SendAsync method with cancellation support."))
                {
                    // Initialize and configure the SMTP client
                    try
                    {
                        using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                        {
                            // Optionally set security options, e.g., TLS
                            client.SecurityOptions = SecurityOptions.Auto;

                            // Send the message asynchronously with the cancellation token
                            await client.SendAsync(message, token);
                            Console.WriteLine("Email sent successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
