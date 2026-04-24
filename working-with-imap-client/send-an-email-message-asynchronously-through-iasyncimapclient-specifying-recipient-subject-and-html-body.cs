using System;
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
            // Placeholder check to avoid real network calls in CI
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, 993, username, password, SecurityOptions.Auto))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = username;
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Subject";
                    message.IsBodyHtml = true;
                    message.HtmlBody = "<h1>Hello</h1><p>This is a test email.</p>";

                    try
                    {
                        // Append (send) the message asynchronously to the default folder
                        string result = await client.AppendMessageAsync(message);
                        Console.WriteLine("Message appended with ID: " + result);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to append message: " + ex.Message);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
