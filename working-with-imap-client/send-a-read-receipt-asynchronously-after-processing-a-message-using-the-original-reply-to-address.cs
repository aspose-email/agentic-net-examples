using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings
            string imapHost = "imap.example.com";
            string imapUsername = "user@example.com";
            string imapPassword = "password";

            // Guard against placeholder credentials to avoid external calls
            if (imapHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping execution.");
                return;
            }

            // Path to the original message file
            string emlPath = "sample.eml";

            // Ensure the EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    var placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body.");
                    placeholder.Save(emlPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the original message
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Determine the Reply-To address; fallback to From address if none
            string replyToAddress;
            if (originalMessage.ReplyToList != null && originalMessage.ReplyToList.Count > 0)
            {
                replyToAddress = originalMessage.ReplyToList[0].Address;
            }
            else
            {
                replyToAddress = originalMessage.From.Address;
            }

            // Create a simple read receipt message
            var receiptMessage = new MailMessage(
                replyToAddress,                 // From (original Reply-To)
                originalMessage.From.Address,  // To (original sender)
                "Read receipt",                // Subject
                "Your message has been read."); // Body

            // Optionally set the Disposition-Notification-To header
            receiptMessage.Headers.Add("Disposition-Notification-To", replyToAddress);

            // Send the receipt asynchronously via IMAP AppendMessageAsync
            try
            {
                using (var client = new ImapClient(imapHost, imapUsername, imapPassword))
                {
                    // Append the receipt to the "Sent" folder
                    string result = await client.AppendMessageAsync("Sent", receiptMessage, CancellationToken.None);
                    Console.WriteLine($"Read receipt appended successfully. Server response: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send read receipt: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
