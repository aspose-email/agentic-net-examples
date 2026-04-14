using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS client connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // External recipient address and subject prefix
            string externalAddress = "external@example.com";
            string subjectPrefix = "[Forwarded] ";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || externalAddress.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages();

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Fetch the original message
                        MailMessage original = client.FetchMessage(messageInfo.UniqueUri);

                        // Compose a new forward message
                        MailMessage forward = new MailMessage();
                        forward.From = original.From;
                        forward.To.Add(new MailAddress(externalAddress));
                        forward.Subject = subjectPrefix + original.Subject;
                        forward.Body = original.Body;

                        // Forward the message using the original message reference
                        client.Forward(forward, messageInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to forward message ID {messageInfo.UniqueUri}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
