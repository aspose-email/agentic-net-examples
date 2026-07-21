using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Ensure TLS 1.2 for secure connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // EWS connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = "example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || domain.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client inside a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, domain))
            {
                try
                {
                    // Get mailbox information to obtain folder URIs
                    var mailboxInfo = client.GetMailboxInfo();

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection inboxMessages = client.ListMessages(mailboxInfo.InboxUri);
                    if (inboxMessages == null || inboxMessages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                        return;
                    }

                    // Process the first message
                    var firstMsgInfo = inboxMessages[0];
                    string messageUri = firstMsgInfo.UniqueUri;

                    // Fetch the full MailMessage
                    MailMessage fetchedMessage = client.FetchMessage(messageUri);
                    if (fetchedMessage == null)
                    {
                        Console.WriteLine("Failed to fetch the message.");
                        return;
                    }

                    // Prepare output file path
                    string outputPath = Path.Combine(Environment.CurrentDirectory, "FetchedMessage.eml");

                    // Guard file I/O
                    try
                    {
                        string dir = Path.GetDirectoryName(outputPath);
                        if (!Directory.Exists(dir))
                        {
                            Console.Error.WriteLine($"Directory does not exist: {dir}");
                            return;
                        }

                        // Save the fetched message to .eml file
                        fetchedMessage.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"IO error: {ioEx.Message}");
                        return;
                    }
                    finally
                    {
                        // Dispose the fetched MailMessage
                        fetchedMessage.Dispose();
                    }

                    // OPTIONAL: Create a draft MAPI message and append it to Drafts folder
                    // (demonstrates AppendMessage with MapiMessage)
                    try
                    {
                        var draft = new MapiMessage("sender@example.com", "recipient@example.com", "Draft Subject", "Draft body content.");
                        client.AppendMessage(mailboxInfo.DraftsUri, draft, true);
                        Console.WriteLine("Draft message appended to Drafts folder.");
                    }
                    catch (Exception draftEx)
                    {
                        Console.Error.WriteLine($"Failed to append draft: {draftEx.Message}");
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS operation error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
