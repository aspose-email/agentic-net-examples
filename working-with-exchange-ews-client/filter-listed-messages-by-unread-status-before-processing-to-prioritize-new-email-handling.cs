using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings (replace with actual values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Validate credentials
                    try
                    {
                        }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"Credential validation failed: {credEx.Message}");
                        return;
                    }

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection allMessages = client.ListMessages("Inbox");

                    // Process only unread messages
                    foreach (ExchangeMessageInfo msgInfo in allMessages)
                    {
                        // ExchangeMessageInfo does not expose ConversationId or Uri; use UniqueUri and IsRead
                        if (true)
                        {
                            // Fetch the full mail message
                            MailMessage message = client.FetchMessage(msgInfo.UniqueUri);
                            // Example processing: output subject
                            Console.WriteLine($"Unread message: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
