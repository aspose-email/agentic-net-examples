using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (string.IsNullOrEmpty(serviceUrl) || serviceUrl.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                        return;
                    }

                    // Iterate through each message info
                    foreach (ExchangeMessageInfo info in messageInfos)
                    {
                        // Fetch the full MailMessage using UniqueUri
                        try
                        {
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                // Example processing: output subject and sender
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message '{info.UniqueUri}': {ex.Message}");
                            // Continue with next message
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
