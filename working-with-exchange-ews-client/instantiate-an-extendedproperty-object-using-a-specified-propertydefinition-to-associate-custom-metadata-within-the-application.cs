using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Ensure TLS 1.2 is used for the EWS connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Exchange Web Services endpoint and credentials (replace with real values)
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client and ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Path to the .msg file to be sent
                string msgPath = "sample.msg";

                // Verify the .msg file exists before attempting to load it
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($".msg file not found: {msgPath}");
                    return;
                }

                // Load the .msg file as a MapiMessage
                MapiMessage mapMsg;
                try
                {
                    mapMsg = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load .msg file: {ex.Message}");
                    return;
                }

                // Retrieve the Drafts folder URI from mailbox information
                string draftsFolderUri;
                try
                {
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    draftsFolderUri = mailboxInfo.DraftsUri;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to obtain mailbox info: {ex.Message}");
                    return;
                }

                // Append the MapiMessage to the Drafts folder; obtain the new item's URI
                string draftUri;
                try
                {
                    draftUri = client.AppendMessage(draftsFolderUri, mapMsg, true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append message to Drafts: {ex.Message}");
                    return;
                }

                // Fetch the created draft as a MailMessage
                MailMessage draftMessage;
                try
                {
                    draftMessage = client.FetchMessage(draftUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch draft message: {ex.Message}");
                    return;
                }

                // Send the draft message
                try
                {
                    client.Send(draftMessage);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
