using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration – replace with real values
            string ewsUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = ""; // leave empty if not needed

            // Output folder for fetched messages
            string outputFolder = "FetchedEmails";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare output folder: {ioEx.Message}");
                return;
            }

            // Create the EWS client (shared mailbox access can be done by specifying the mailbox URI of the shared mailbox)
            IEWSClient client = null;
            try
            {
                if (string.IsNullOrEmpty(domain))
                {
                    client = EWSClient.GetEWSClient(ewsUrl, username, password);
                }
                else
                {
                    client = EWSClient.GetEWSClient(ewsUrl, username, password, domain);
                }
            }
            catch (Exception connEx)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {connEx.Message}");
                return;
            }

            // Use the client within a using block to ensure proper disposal
            using (client)
            {
                try
                {
                    // Retrieve mailbox information (folders URIs)
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Access the Inbox folder of the shared mailbox
                    string inboxUri = mailboxInfo.InboxUri;

                    // List messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    int index = 0;
                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        try
                        {
                            // Fetch the full message
                            MailMessage message = client.FetchMessage(msgInfo.UniqueUri);

                            // Save the message as .eml
                            string filePath = Path.Combine(outputFolder, $"Message_{index}.eml");
                            message.Save(filePath);
                            Console.WriteLine($"Saved message to {filePath}");
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch or save message {msgInfo.UniqueUri}: {msgEx.Message}");
                        }
                        finally
                        {
                            index++;
                        }
                    }
                }
                catch (Exception opEx)
                {
                    Console.Error.WriteLine($"Operation failed: {opEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
