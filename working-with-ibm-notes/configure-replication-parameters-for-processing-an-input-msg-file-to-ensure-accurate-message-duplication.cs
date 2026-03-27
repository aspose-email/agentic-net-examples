using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Input MSG file path (adjust as needed)
            string msgPath = "input.msg";

            // Guard file existence
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage and convert to MailMessage
            MailMessage mailMessage;
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Convert with default options
                mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
            }

            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the ImapClient inside a using block
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Connection safety guard
                try
                {
                    imapClient.Noop();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP connection check failed: {ex.Message}");
                    return;
                }

                // Append the message to the desired folder (e.g., "Inbox")
                try
                {
                    // Use the overload that accepts a MailMessage directly
                    string resultUri = await imapClient.AppendMessageAsync("Inbox", mailMessage);
                    Console.WriteLine($"Message appended successfully. URI: {resultUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error appending message: {ex.Message}");
                }
            }

            // Dispose the MailMessage explicitly
            mailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
