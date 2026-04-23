using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

            // Guard against placeholder credentials to avoid live network calls.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail access.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            if (gmailClient == null)
            {
                Console.Error.WriteLine("Gmail client is null.");
                return;
            }

            // Use the client within a using block to ensure disposal.
            using (gmailClient)
            {
                // Retrieve the list of messages.
                List<GmailMessageInfo> messageInfos = null;
                try
                {
                    messageInfos = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                if (messageInfos == null)
                {
                    Console.Error.WriteLine("No messages retrieved.");
                    return;
                }

                // Prepare CSV output.
                string csvPath = "urls.csv";
                string csvDirectory = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(csvDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{csvDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Write URLs to CSV.
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath, false))
                    {
                        writer.WriteLine("MessageId,Url");
                        foreach (GmailMessageInfo info in messageInfos)
                        {
                            // Fetch the full message.
                            MailMessage message = null;
                            try
                            {
                                message = gmailClient.FetchMessage(info.Id);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to fetch message '{info.Id}': {ex.Message}");
                                continue;
                            }

                            if (message == null)
                            {
                                continue;
                            }

                            using (message)
                            {
                                // Prefer HTML body; fall back to plain text if needed.
                                string bodyContent = message.HtmlBody;
                                if (string.IsNullOrEmpty(bodyContent))
                                {
                                    bodyContent = message.Body;
                                }

                                if (string.IsNullOrEmpty(bodyContent))
                                {
                                    continue;
                                }

                                // Extract URLs using a regular expression.
                                Regex urlRegex = new Regex(@"https?://[^\s'""]+", RegexOptions.IgnoreCase);
                                MatchCollection matches = urlRegex.Matches(bodyContent);
                                foreach (Match match in matches)
                                {
                                    string url = match.Value.TrimEnd('\'', '"', ')', '>', '.');
                                    // Escape double quotes in URL for CSV compliance.
                                    string escapedUrl = url.Replace("\"", "\"\"");
                                    writer.WriteLine($"{info.Id},\"{escapedUrl}\"");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSV file '{csvPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
