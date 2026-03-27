using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            // Top-level exception guard
            try
            {
                // Define mailbox URI and credentials (replace with real values or keep placeholders)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Ensure the output directory exists before saving any files
                string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create and use the EWS client inside a using block for proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // List messages from the Inbox folder
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                    Console.WriteLine($"Total messages in Inbox: {messageInfos.Count}");

                    // Process each message
                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full mail message
                        using (MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {mailMessage.Subject}");

                            // Optionally save the message to a .eml file
                            string emlPath = Path.Combine(outputDirectory, $"{messageInfo.UniqueUri}.eml");
                            // Guard file write operation
                            try
                            {
                                mailMessage.Save(emlPath, SaveOptions.DefaultEml);
                                Console.WriteLine($"Saved message to: {emlPath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{messageInfo.UniqueUri}': {ioEx.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Friendly error output
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
