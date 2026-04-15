using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information to get the Inbox folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // Create a MAPI message (4‑argument constructor required)
                    MapiMessage mapiMessage = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Sample Subject",
                        "This is a sample message body."
                    );

                    // Add a custom property to indicate processing status
                    string propertyName = "ProcessingStatus";
                    string propertyValue = "Processed";

                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    byte[] valueBytes = System.Text.Encoding.Unicode.GetBytes(propertyValue);
                    mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, propertyName);

                    // Append the message to the Inbox folder as a draft (markAsSent = false)
                    string messageUri = client.AppendMessage(inboxUri, mapiMessage, false);
                    Console.WriteLine($"Message appended with URI: {messageUri}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
