using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve the list of message infos in the default folder.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(CancellationToken.None);

                    // Collect unique IDs of all messages.
                    List<string> uids = new List<string>();
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        if (!string.IsNullOrEmpty(info.UniqueId))
                        {
                            uids.Add(info.UniqueId);
                        }
                    }

                    if (uids.Count == 0)
                    {
                        Console.WriteLine("No messages found.");
                        return;
                    }

                    // Fetch the full messages asynchronously.
                    IList<MailMessage> messages = await client.FetchMessagesAsync(uids, CancellationToken.None);

                    // Regex to extract order numbers (e.g., "Order #12345").
                    Regex orderRegex = new Regex(@"Order\s*#\s*(\d+)", RegexOptions.IgnoreCase);

                    foreach (MailMessage message in messages)
                    {
                        string body = message.Body ?? string.Empty;
                        Match match = orderRegex.Match(body);
                        if (match.Success)
                        {
                            string orderNumber = match.Groups[1].Value;
                            Console.WriteLine($"Found order number: {orderNumber} in message with Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
