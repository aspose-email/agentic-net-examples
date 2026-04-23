using Aspose.Email.Clients.Activity;
using Aspose.Email.Clients;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailWebhookExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Configuration (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                string folder = "INBOX";
                string webhookUrl = "https://example.com/webhook";

                // Skip execution when placeholder values are detected
                if (host.Contains("example.com") || webhookUrl.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder configuration detected. Skipping execution.");
                    return;
                }

                // Create and use the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the target folder (lightweight validation of credentials)
                    client.SelectFolder(folder);

                    // Retrieve the list of messages in the folder
                    Aspose.Email.Clients.Imap.ImapMessageInfoCollection messagesInfo = client.ListMessages();

                    // Reuse a single HttpClient instance for all webhook calls
                    using (HttpClient httpClient = new HttpClient())
                    {
                        foreach (Aspose.Email.Clients.Imap.ImapMessageInfo info in messagesInfo)
                        {
                            // Fetch the full message
                            MailMessage message = client.FetchMessage(info.UniqueId);

                            // Prepare JSON payload with subject and sender
                            var payload = new
                            {
                                subject = message.Subject,
                                sender = message.From.ToString()
                            };
                            string json = JsonSerializer.Serialize(payload);
                            using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                            {
                                // Send the webhook asynchronously
                                HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
                                if (!response.IsSuccessStatusCode)
                                {
                                    Console.Error.WriteLine($"Webhook failed for message {info.UniqueId}: {response.StatusCode}");
                                }
                                else
                                {
                                    Console.WriteLine($"Webhook sent for message {info.UniqueId}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
