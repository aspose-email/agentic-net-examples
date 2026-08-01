using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // ----- Configuration -----
            string ewsUrl = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = ""; // optional, leave empty if not needed

            // Guard against placeholder endpoint
            if (string.IsNullOrWhiteSpace(ewsUrl) || ewsUrl.Contains("your.exchange.server"))
            {
                Console.Error.WriteLine("EWS endpoint is not configured. Please provide a valid URL.");
                return;
            }

            // Guard against placeholder credentials
            bool placeholderCreds = string.IsNullOrWhiteSpace(username) ||
                                    username.Contains("example.com") ||
                                    string.IsNullOrWhiteSpace(password) ||
                                    password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (placeholderCreds)
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // ----- Connect to Exchange -----
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential(username, password, domain)))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // List messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Identify unwanted messages (example: subject contains "Unwanted")
                        if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                            messageInfo.Subject.IndexOf("Unwanted", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            try
                            {
                                // Delete the message permanently
                                client.DeleteItem(messageInfo.UniqueUri, DeletionOptions.DeletePermanently);
                                Console.WriteLine($"Deleted message: {messageInfo.Subject}");
                            }
                            catch (Exception exDelete)
                            {
                                Console.Error.WriteLine($"Failed to delete message '{messageInfo.Subject}': {exDelete.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception exConn)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {exConn.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
