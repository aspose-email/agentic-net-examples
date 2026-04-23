using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual use.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string defaultEmail = "user@example.com";

            // Skip real network calls when placeholders are detected.
            if (clientId.StartsWith("your_") ||
                clientSecret.StartsWith("your_") ||
                refreshToken.StartsWith("your_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail statistics retrieval.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Retrieve all messages in the mailbox.
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();

                    int totalMessages = messages.Count;

                    // Determine unread count (fallback to label check if IsRead not available).
                    int unreadCount = messages.Count(m =>
                    {
                        var type = m.GetType();

                        var isReadProp = type.GetProperty("IsRead");
                        if (isReadProp != null && isReadProp.PropertyType == typeof(bool))
                        {
                            return !(bool)isReadProp.GetValue(m);
                        }

                        var labelProp = type.GetProperty("LabelIds");
                        if (labelProp != null)
                        {
                            var labels = labelProp.GetValue(m) as IEnumerable<string>;
                            return labels != null && labels.Contains("UNREAD");
                        }

                        return false;
                    });

                    // Aggregate size per label.
                    var sizePerLabel = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

                    foreach (var msg in messages)
                    {
                        long size = 0;
                        var sizeProp = msg.GetType().GetProperty("Size");
                        if (sizeProp != null && sizeProp.PropertyType == typeof(long))
                        {
                            size = (long)sizeProp.GetValue(msg);
                        }

                        var labelProp = msg.GetType().GetProperty("LabelIds");
                        IEnumerable<string> labels = null;
                        if (labelProp != null)
                        {
                            labels = labelProp.GetValue(msg) as IEnumerable<string>;
                        }

                        if (labels == null || !labels.Any())
                        {
                            const string noLabel = "NoLabel";
                            if (!sizePerLabel.ContainsKey(noLabel))
                                sizePerLabel[noLabel] = 0;
                            sizePerLabel[noLabel] += size;
                        }
                        else
                        {
                            foreach (var label in labels)
                            {
                                if (!sizePerLabel.ContainsKey(label))
                                    sizePerLabel[label] = 0;
                                sizePerLabel[label] += size;
                            }
                        }
                    }

                    // Output statistics.
                    Console.WriteLine($"Total messages: {totalMessages}");
                    Console.WriteLine($"Unread messages: {unreadCount}");
                    Console.WriteLine("Size per label (bytes):");
                    foreach (var kvp in sizePerLabel)
                    {
                        Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving Gmail statistics: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
