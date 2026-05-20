using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Guard against placeholder credentials.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Gmail client credentials are placeholders. Skipping send operation.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);
            try
            {
                // Compose email.
                MailMessage message = new MailMessage
                {
                    From = "sender@example.com",
                    Subject = "Filtered Content Example"
                };
                message.To.Add("recipient@example.com");

                // Original body with potential prohibited words.
                string originalBody = "Hello, this email contains badword1 and some other text.";
                // List of prohibited words to filter out.
                List<string> prohibitedWords = new List<string> { "badword1", "badword2" };
                // Apply filter: replace each prohibited word with asterisks.
                string filteredBody = originalBody;
                foreach (string word in prohibitedWords)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        filteredBody = filteredBody.Replace(word, new string('*', word.Length), StringComparison.OrdinalIgnoreCase);
                    }
                }
                message.Body = filteredBody;

                // Send the filtered message.
                gmailClient.SendMessage(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                if (gmailClient is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
