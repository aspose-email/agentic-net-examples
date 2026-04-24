using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapSentiment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip real network calls when placeholders are used
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected – IMAP operations are skipped.");
                    return;
                }

                // Create and use the IMAP client inside a using block
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Validate credentials safely
                    bool credentialsValid = await client.ValidateCredentialsAsync();
                    if (!credentialsValid)
                    {
                        Console.Error.WriteLine("IMAP authentication failed.");
                        return;
                    }

                    // Select the INBOX folder (default folder if not selected)
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages asynchronously
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync();

                    // Process each message
                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message asynchronously
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                        // Perform a very simple sentiment analysis on the body text
                        int sentimentScore = AnalyzeSentiment(message.Body);

                        // Output the result – higher score means more positive sentiment
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"Sentiment Score: {sentimentScore}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Friendly error output
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Very basic sentiment analysis: counts occurrences of positive and negative words
        private static int AnalyzeSentiment(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            string[] positiveWords = { "good", "great", "excellent", "happy", "love", "awesome", "fantastic" };
            string[] negativeWords = { "bad", "poor", "terrible", "sad", "hate", "awful", "worst" };

            int score = 0;
            string lowerText = text.ToLowerInvariant();

            foreach (string word in positiveWords)
            {
                if (lowerText.Contains(word))
                    score++;
            }

            foreach (string word in negativeWords)
            {
                if (lowerText.Contains(word))
                    score--;
            }

            return score;
        }
    }
}
