using Aspose.Email.Mapi;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Wrap client creation/connection in its own try/catch
            try
            {
                // Initialize EWS client (replace with real server/credentials)
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Create a simple mail message to archive
                    MailMessage message = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Test Subject",
                        "Test Body");

                    // Get the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // Append the message to the Inbox (markAsSent = false creates a draft)
                    string messageUri = client.AppendMessage(inboxUri, MapiMessage.FromMailMessage(message), false);

                    // Attempt to archive the newly added message
                    try
                    {
                        // ArchiveItem(sourceFolderUri, uniqueId) moves the item to the archive mailbox
                        client.ArchiveItem(inboxUri, messageUri);
                        Console.WriteLine("Message archived successfully.");
                    }
                    catch (ExchangeException ex)
                    {
                        // Log detailed EWS error information
                        Console.Error.WriteLine($"Archive operation failed: {ex.Message}");
                        Console.Error.WriteLine($"Error details: {ex.ErrorDetails}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log client‑initialisation or connection errors
                Console.Error.WriteLine($"EWS client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
