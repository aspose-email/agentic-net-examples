using Aspose.Email.Clients.Exchange.Dav;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        const string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        const string userName = "username";
        const string password = "password";

        if (IsPlaceholder(serverUrl) || IsPlaceholder(userName) || IsPlaceholder(password))
        {
            Console.WriteLine("Placeholder credentials detected. Skipping external Exchange operations.");
            return;
        }

        try
        {
            using (ExchangeClient client = new ExchangeClient(serverUrl, userName, password))
            {
                // Retrieve all messages from the Inbox folder (include subfolders = true)
                ExchangeMessageInfoCollection messages = client.ListMessages(userName, "Inbox");

                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    // Filter: delivery notifications usually contain "Delivery" in the subject
                    if (!string.IsNullOrEmpty(msgInfo.Subject) &&
                        msgInfo.Subject.IndexOf("Delivery", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                        Console.WriteLine($"Date: {msgInfo.InternalDate}");

                        // Fetch the full message if further processing is required
                        using (MailMessage fullMessage = client.FetchMessage(msgInfo.UniqueUri))
                        {
                            string body = fullMessage.Body ?? string.Empty;
                            int previewLength = Math.Min(100, body.Length);
                            Console.WriteLine($"Body preview: {body.Substring(0, previewLength)}");
                        }

                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while accessing Exchange: {ex.Message}");
        }
    }

    private static bool IsPlaceholder(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string lowered = value.Trim().ToLowerInvariant();
        return lowered.Contains("example") ||
               lowered.Contains("username") ||
               lowered.Contains("password");
    }
}
