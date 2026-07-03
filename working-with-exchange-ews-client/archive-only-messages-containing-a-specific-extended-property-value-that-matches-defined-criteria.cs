using Aspose.Email.Storage.Pst;
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
            // EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (IEWSClient) using the static factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Retrieve information about the Inbox folder
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo("Inbox");
                string inboxUri = inboxInfo.Uri;

                // List all messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);

                // Define the extended property name and the value that qualifies a message for archiving
                const string extendedPropertyName = "X-Archive-Flag";
                const string requiredValue = "true";

                // Iterate through each message, fetch its full content, and check the extended property
                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the complete message (including headers) using its unique URI
                    MailMessage mail = client.FetchMessage(messageInfo.UniqueUri);

                    // Retrieve the value of the custom header (extended property)
                    string propertyValue = mail.Headers[extendedPropertyName];

                    // If the property matches the required value, archive the message
                    if (string.Equals(propertyValue, requiredValue, StringComparison.OrdinalIgnoreCase))
                    {
                        // Archive the message by moving it to the archive mailbox
                        client.ArchiveItem(inboxUri, messageInfo.UniqueUri);
                        Console.WriteLine($"Archived message: {mail.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream without crashing the application
            Console.Error.WriteLine(ex.Message);
        }
    }
}
