using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Output file for the modified message
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "ModifiedMessage.eml");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Build a simple query to find a message (e.g., subject contains "Test")
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Test");
                MailQuery query = builder.GetQuery();

                // List messages matching the query
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found matching the query.");
                    return;
                }

                // Fetch the first message
                ExchangeMessageInfo messageInfo = messages[0];
                MailMessage mailMessage;
                try
                {
                    mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message: {ex.Message}");
                    return;
                }

                // Convert to MapiMessage to manipulate extended properties
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Add a custom extended property (Unicode string)
                string customPropName = "X-MyCustomProp";
                string customPropValue = "CustomValue";
                byte[] valueBytes = Encoding.UTF8.GetBytes(customPropValue);
                mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, customPropName);

                // Save the modified message to a local file
                try
                {
                    mapiMessage.Save(outputPath);
                    Console.WriteLine($"Modified message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified message: {ex.Message}");
                    return;
                }

                // Optionally upload the modified message back to a folder (e.g., "Inbox")
                try
                {
                    // Convert back to MailMessage for upload
                    MailMessage modifiedMail = mapiMessage.ToMailMessage(new MailConversionOptions());
                    client.AppendMessage(client.MailboxInfo.InboxUri, modifiedMail);
                    Console.WriteLine("Modified message uploaded back to the mailbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to upload modified message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}