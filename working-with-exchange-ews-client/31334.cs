using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attachment files (create minimal placeholders if missing)
            string attachmentPath1 = "attachment1.txt";
            string attachmentPath2 = "attachment2.txt";

            if (!File.Exists(attachmentPath1))
                File.WriteAllText(attachmentPath1, "Placeholder content 1");
            if (!File.Exists(attachmentPath2))
                File.WriteAllText(attachmentPath2, "Placeholder content 2");

            // Create the mail message and add attachments
            MailMessage message = new MailMessage(
                "sender@example.com",
                "receiver@example.com",
                "Test Message with Attachments",
                "This is a test email containing multiple attachments.");

            message.Attachments.Add(new Attachment(attachmentPath1));
            message.Attachments.Add(new Attachment(attachmentPath2));

            // Exchange client configuration
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, credentials))
            {
                try
                {
                    // Send the message
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                    return;
                }

                // Verify the sent message in Sent Items folder
                string sentFolderUri = client.MailboxInfo.SentItemsUri;
                if (string.IsNullOrEmpty(sentFolderUri))
                {
                    Console.Error.WriteLine("Sent Items folder URI not available.");
                    return;
                }

                ExchangeMessageInfoCollection sentMessages;
                try
                {
                    sentMessages = client.ListMessages(sentFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing sent messages: {ex.Message}");
                    return;
                }

                if (sentMessages == null || sentMessages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in Sent Items.");
                    return;
                }

                // Fetch the most recent sent message (assumed to be the first in the collection)
                ExchangeMessageInfo latestInfo = sentMessages[0];
                MailMessage fetchedMessage;
                try
                {
                    fetchedMessage = client.FetchMessage(latestInfo.UniqueUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching sent message: {ex.Message}");
                    return;
                }

                // Verify attachment count and names
                if (fetchedMessage.Attachments.Count != 2)
                {
                    Console.Error.WriteLine("Attachment count mismatch.");
                }
                else
                {
                    bool attachment1Found = false;
                    bool attachment2Found = false;

                    foreach (Attachment att in fetchedMessage.Attachments)
                    {
                        if (att.Name.Equals(Path.GetFileName(attachmentPath1), StringComparison.OrdinalIgnoreCase))
                            attachment1Found = true;
                        if (att.Name.Equals(Path.GetFileName(attachmentPath2), StringComparison.OrdinalIgnoreCase))
                            attachment2Found = true;
                    }

                    if (attachment1Found && attachment2Found)
                        Console.WriteLine("Both attachments verified successfully.");
                    else
                        Console.Error.WriteLine("One or more attachments could not be verified.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
