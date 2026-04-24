using Aspose.Email.Clients.Activity;
using Aspose.Email.Clients;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials and server details
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string webhookUrl = "https://example.com/webhook";

            // Skip real network calls if placeholders are detected
            if (host.Contains("example.com") || webhookUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder values detected. Skipping network operations.");
                return;
            }

            // Create and connect the IMAP client safely
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    // Prepare an HttpClient for webhook calls
                    using (HttpClient httpClient = new HttpClient())
                    {
                        foreach (ImapMessageInfo info in messageInfos)
                        {
                            // Fetch the full mail message
                            MailMessage mailMessage = client.FetchMessage(info.UniqueId);

                            // Extract required metadata
                            string subject = mailMessage.Subject ?? string.Empty;
                            string sender = mailMessage.From != null ? mailMessage.From.ToString() : string.Empty;

                            // Send webhook asynchronously
                            await SendWebhookAsync(httpClient, webhookUrl, subject, sender);
                        }
                    }
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

    // Sends a JSON payload containing subject and sender to the specified webhook URL
    private static async Task SendWebhookAsync(HttpClient httpClient, string url, string subject, string sender)
    {
        try
        {
            var payload = new
            {
                Subject = subject,
                Sender = sender
            };
            string json = System.Text.Json.JsonSerializer.Serialize(payload);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Webhook POST failed: {ex.Message}");
        }
    }
}
