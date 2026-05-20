using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details – skip real network call if placeholders are used
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                client.SecurityOptions = SecurityOptions.Auto;

                // Prepare a simple email message
                MailMessage message = new MailMessage
                {
                    From = "sender@example.com",
                    Subject = "Test email with Retry-After handling",
                    Body = "This email demonstrates handling of the SMTP Retry-After header."
                };
                message.To.Add("recipient@example.com");

                const int maxAttempts = 2;
                int attempt = 0;

                while (attempt < maxAttempts)
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                        break; // Success, exit loop
                    }
                    catch (SmtpException ex)
                    {
                        attempt++;

                        // Check for 421 Service Not Available which may include a Retry-After header
                        if (ex.StatusCode == SmtpStatusCode.ServiceNotAvailable && attempt < maxAttempts)
                        {
                            int retrySeconds = ParseRetryAfter(ex);
                            if (retrySeconds > 0)
                            {
                                Console.WriteLine($"Server requested retry after {retrySeconds} seconds. Waiting...");
                                Thread.Sleep(retrySeconds * 1000);
                                continue; // Retry sending
                            }
                        }

                        // If not a retryable condition or max attempts reached, report error
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }

    // Attempts to extract the Retry-After value (seconds) from the exception details.
    private static int ParseRetryAfter(SmtpException ex)
    {
        // The Retry-After value may be present in the ErrorDetails or Message string.
        string details = ex.ErrorDetails?.ToString() ?? ex.Message;
        if (string.IsNullOrEmpty(details))
            return 0;

        // Look for a line like "Retry-After: 120"
        foreach (string line in details.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith("Retry-After:", StringComparison.OrdinalIgnoreCase))
            {
                string valuePart = line.Substring("Retry-After:".Length).Trim();

                // Try parsing as integer seconds
                if (int.TryParse(valuePart, out int seconds))
                    return seconds;

                // Try parsing as a date/time
                if (DateTime.TryParse(valuePart, out DateTime retryTime))
                {
                    int diff = (int)(retryTime - DateTime.UtcNow).TotalSeconds;
                    return diff > 0 ? diff : 0;
                }
            }
        }

        return 0;
    }
}
