using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.gmail.com";
            int port = 993;
            string username = "your.email@gmail.com";
            string password = "yourpassword";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com") ||
                username.StartsWith("your.", StringComparison.OrdinalIgnoreCase) ||
                password.StartsWith("your", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create a new mail message with HTML body
            MailMessage message = new MailMessage();
            message.From = username;
            message.To.Add(username);
            message.Subject = "Test Draft Message";
            message.HtmlBody = "<html><body><h1>Hello from Aspose.Email!</h1></body></html>";

            // Connect to Gmail via IMAP and append the message to the Drafts folder
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Gmail Drafts folder name
                    const string draftsFolder = "[Gmail]/Drafts";

                    string appendedId = client.AppendMessage(draftsFolder, message);
                    Console.WriteLine($"Message appended to Drafts. ID: {appendedId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
