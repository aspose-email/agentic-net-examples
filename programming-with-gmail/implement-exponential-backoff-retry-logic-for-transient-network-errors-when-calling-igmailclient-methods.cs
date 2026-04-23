using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Detect placeholder credentials and exit gracefully.
            if (accessToken.StartsWith("YOUR_") || string.IsNullOrWhiteSpace(defaultEmail))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            // Prepare a simple email message.
            MailMessage message = new MailMessage();
            message.From = defaultEmail;
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email with Exponential Backoff";
            message.Body = "This email was sent using Aspose.Email with retry logic.";

            // Exponential backoff parameters.
            const int maxRetries = 5;
            const int baseDelayMilliseconds = 1000; // 1 second

            // Attempt to send the message with retry logic.
            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    // Send the message.
                    string messageId = gmailClient.SendMessage(message);
                    Console.WriteLine($"Message sent successfully. Id: {messageId}");
                    break; // Success, exit the retry loop.
                }
                catch (GoogleClientException ex) // Transient network errors are reported as GoogleClientException.
                {
                    Console.Error.WriteLine($"Attempt {attempt + 1} failed: {ex.Message}");

                    if (attempt == maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Giving up.");
                        break;
                    }

                    // Calculate exponential backoff delay.
                    int delay = (int)Math.Pow(2, attempt) * baseDelayMilliseconds;
                    Console.WriteLine($"Waiting {delay} ms before next retry...");
                    Thread.Sleep(delay);
                }
                catch (AsposeException ex) // Catch other Aspose exceptions that may be transient.
                {
                    Console.Error.WriteLine($"Attempt {attempt + 1} failed: {ex.Message}");

                    if (attempt == maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Giving up.");
                        break;
                    }

                    int delay = (int)Math.Pow(2, attempt) * baseDelayMilliseconds;
                    Console.WriteLine($"Waiting {delay} ms before next retry...");
                    Thread.Sleep(delay);
                }
            }

            // Dispose of the client explicitly.
            if (gmailClient is IDisposable disposableClient)
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
