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
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against running with placeholder credentials.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR_ACCESS_TOKEN"))
            {
                Console.Error.WriteLine("Please provide valid Gmail access token and email address.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Prepare a simple email message.
                using (MailMessage message = new MailMessage())
                {
                    message.From = defaultEmail;
                    message.To.Add(defaultEmail);
                    message.Subject = "Test Email with Exponential Backoff";
                    message.Body = "This email was sent using Aspose.Email with retry logic.";

                    const int maxRetries = 5;
                    int attempt = 0;
                    int delayMilliseconds = 1000; // Initial backoff delay.

                    while (true)
                    {
                        try
                        {
                            // Attempt to send the message.
                            string messageId = gmailClient.SendMessage(message);
                            Console.WriteLine($"Message sent successfully. Id: {messageId}");
                            break; // Success – exit retry loop.
                        }
                        catch (GoogleClientException ex)
                        {
                            // Consider this a transient error and retry.
                            attempt++;
                            if (attempt > maxRetries)
                            {
                                Console.Error.WriteLine($"Failed after {maxRetries} attempts: {ex.Message}");
                                break;
                            }

                            Console.Error.WriteLine($"Transient error encountered (attempt {attempt}): {ex.Message}");
                            Console.Error.WriteLine($"Waiting {delayMilliseconds} ms before retrying...");

                            Thread.Sleep(delayMilliseconds);
                            delayMilliseconds *= 2; // Exponential backoff.
                        }
                        catch (Exception ex)
                        {
                            // Non‑transient error – report and abort.
                            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                            break;
                        }
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
