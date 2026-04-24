using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Skipping IMAP operation due to placeholder credentials/host.");
                return;
            }

            // Create a simple draft mail message
            MailMessage draftMessage = new MailMessage();
            draftMessage.From = new MailAddress("sender@example.com");
            draftMessage.To.Add(new MailAddress("recipient@example.com"));
            draftMessage.Subject = "Draft Subject";
            draftMessage.Body = "This is a draft email.";

            // Original flags to preserve (e.g., Draft flag)
            ImapMessageFlags originalFlags = ImapMessageFlags.Draft;

            // Connect to IMAP server and append the draft asynchronously
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Append the message to the Drafts folder
                string appendedMessageId = await client.AppendMessageAsync("Drafts", draftMessage, CancellationToken.None);

                // Preserve original flags by adding them back to the uploaded message
                await client.AddMessageFlagsAsync(appendedMessageId, originalFlags, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
