using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace with your actual Exchange Web Services URL and credentials
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client using the factory method (EWSClient is abstract)
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information (folders URIs, etc.)
                    ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection inboxMessages = ewsClient.ListMessages(mailboxInfo.InboxUri);
                    Console.WriteLine($"Inbox contains {inboxMessages.Count} message(s).");

                    if (inboxMessages.Count > 0)
                    {
                        // Fetch the first message from the Inbox
                        MailMessage firstMessage = ewsClient.FetchMessage(inboxMessages[0].UniqueUri);

                        // Define output file path
                        string outputPath = "output.eml";

                        // Save the fetched message to disk, handling any I/O errors
                        try
                        {
                            firstMessage.Save(outputPath);
                            Console.WriteLine($"First message saved to '{outputPath}'.");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard – report errors without crashing
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
