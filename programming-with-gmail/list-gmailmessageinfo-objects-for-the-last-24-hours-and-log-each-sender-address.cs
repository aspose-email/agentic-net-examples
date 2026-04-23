using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid external calls during CI.
                if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR") ||
                    string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail operation.");
                    return;
                }

                // Create Gmail client instance.
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {clientEx.Message}");
                    return;
                }

                // Ensure the client is disposed properly.
                using (gmailClient)
                {
                    List<GmailMessageInfo> messageInfos = null;
                    try
                    {
                        // Retrieve all message identifiers.
                        messageInfos = gmailClient.ListMessages();
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Error listing Gmail messages: {listEx.Message}");
                        return;
                    }

                    DateTime utcCutoff = DateTime.UtcNow.AddHours(-24);

                    foreach (GmailMessageInfo messageInfo in messageInfos)
                    {
                        MailMessage mailMessage = null;
                        try
                        {
                            // Fetch the full message to access its Date and Sender.
                            mailMessage = gmailClient.FetchMessage(messageInfo.Id);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageInfo.Id}: {fetchEx.Message}");
                            continue;
                        }

                        // Dispose the fetched message after use.
                        using (mailMessage)
                        {
                            // Filter messages received in the last 24 hours.
                            if (mailMessage.Date.ToUniversalTime() >= utcCutoff)
                            {
                                string senderAddress = mailMessage.Sender?.Address ?? "Unknown Sender";
                                Console.WriteLine(senderAddress);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
