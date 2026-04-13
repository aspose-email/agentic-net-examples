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
            // Initialize EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Identifier of the message to be updated and archived
                string messageUri = "message-uri-placeholder";

                // Fetch the message as a MAPI message
                MapiMessage mapiMessage = client.FetchItem(messageUri);

                // Update a custom extended property on the message
                string propertyName = "MyCustomProperty";
                string propertyValue = "UpdatedValue";
                mapiMessage.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    Encoding.Unicode.GetBytes(propertyValue),
                    propertyName);

                // Archive the updated message (using the Inbox folder as source)
                string sourceFolderUri = client.MailboxInfo.InboxUri;
                client.ArchiveItem(sourceFolderUri, mapiMessage);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
