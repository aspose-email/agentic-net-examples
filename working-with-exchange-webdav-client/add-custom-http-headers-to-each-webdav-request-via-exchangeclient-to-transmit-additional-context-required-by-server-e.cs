using System;
using System.Collections.Specialized;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Add custom HTTP headers via reflection (property may be internal or absent in some versions)
                    PropertyInfo httpHeadersProp = client.GetType().GetProperty("HttpHeaders",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (httpHeadersProp != null)
                    {
                        var headers = httpHeadersProp.GetValue(client) as NameValueCollection;
                        if (headers != null)
                        {
                            headers.Add("X-Custom-Context", "MyValue");
                            headers.Add("X-Request-ID", Guid.NewGuid().ToString());
                        }
                    }

                    // List a few messages from the Inbox (headers are applied automatically if supported)
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri, 5);
                    Console.WriteLine($"Retrieved {messageInfos.Count} message(s) from the Inbox.");

                    foreach (var info in messageInfos)
                    {
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
