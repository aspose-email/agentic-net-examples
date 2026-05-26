using System;
using System.IO;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder mailbox URI and credentials.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Skip real network calls when placeholders are used.
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder configuration detected. Skipping execution.");
                return;
            }

            // Ensure the message file exists; create a minimal placeholder if missing.
            string messagePath = "message.eml";
            try
            {
                if (!File.Exists(messagePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To.Add("recipient@example.com");
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder email.";
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Load the email message.
            MailMessage message;
            try
            {
                message = MailMessage.Load(messagePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
                return;
            }

            // Create the Exchange client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, credentials))
                {
                    // Retry strategy parameters.
                    const int maxRetries = 5;
                    int initialDelayMs = 1000; // 1 second
                    int attempt = 0;
                    int delayMs = initialDelayMs;

                    while (true)
                    {
                        try
                        {
                            // Attempt to send the message.
                            client.Send(message);
                            Console.WriteLine("Message sent successfully.");
                            break; // Success, exit loop.
                        }
                        catch (Exception ex)
                        {
                            attempt++;
                            if (attempt > maxRetries)
                            {
                                Console.Error.WriteLine($"Operation failed after {maxRetries} retries: {ex.Message}");
                                break;
                            }

                            // Look for a "Retry-After" hint in the exception message.
                            int retryAfterSeconds = 0;
                            string msg = ex.Message ?? string.Empty;
                            int idx = msg.IndexOf("Retry-After:", StringComparison.OrdinalIgnoreCase);
                            if (idx >= 0)
                            {
                                string after = msg.Substring(idx + "Retry-After:".Length).Trim();
                                int spaceIdx = after.IndexOf(' ');
                                if (spaceIdx > 0) after = after.Substring(0, spaceIdx);
                                int.TryParse(after, out retryAfterSeconds);
                            }

                            // Determine wait time.
                            int waitMs = retryAfterSeconds > 0 ? retryAfterSeconds * 1000 : delayMs;
                            Console.WriteLine($"Retry {attempt}/{maxRetries} after {waitMs} ms due to error: {ex.Message}");
                            Thread.Sleep(waitMs);

                            // Exponential backoff for subsequent attempts.
                            delayMs *= 2;
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Client error: {clientEx.Message}");
                return;
            }
            finally
            {
                // Dispose the loaded message.
                message?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
