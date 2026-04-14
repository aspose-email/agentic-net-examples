using System;
using System.Net;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Server connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a new MAPI message
                MapiMessage mapiMessage = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is the body of the message.");

                // Add a custom extended property (Unicode string)
                string propertyName = "MyCustomProp";
                string propertyValue = "SampleValue";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                byte[] valueBytes = Encoding.Unicode.GetBytes(propertyValue);
                mapiMessage.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    valueBytes,
                    propertyName);

                // Get the Inbox folder URI
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // Append the message to the Inbox as a sent item
                client.AppendMessage(inboxFolderUri, mapiMessage, true);
                Console.WriteLine("Message with extended property uploaded successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
