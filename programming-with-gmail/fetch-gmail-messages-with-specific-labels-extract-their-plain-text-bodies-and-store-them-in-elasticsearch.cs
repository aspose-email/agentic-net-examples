using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailToElasticsearch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder values – replace with real credentials or keep as placeholders for CI safety
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "YOUR_EMAIL@example.com";
                string elasticsearchUrl = "YOUR_ELASTICSEARCH_URL";
                string indexName = "gmail_messages";

                // Guard against placeholder literals
                if (accessToken.StartsWith("YOUR_") ||
                    defaultEmail.StartsWith("YOUR_") ||
                    elasticsearchUrl.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Placeholder credentials or URLs detected. Execution skipped.");
                    return;
                }

                // Create Gmail client
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                using (gmailClient)
                {
                    // Retrieve list of messages (all messages – label filtering can be added if API supports)
                    List<GmailMessageInfo> messageInfos = null;
                    try
                    {
                        messageInfos = gmailClient.ListMessages();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                        return;
                    }

                    // Prepare HTTP client for Elasticsearch (will be used only if URL is valid)
                    using (HttpClient httpClient = new HttpClient())
                    {
                        foreach (GmailMessageInfo info in messageInfos)
                        {
                            // Fetch full message
                            MailMessage mailMessage = null;
                            try
                            {
                                mailMessage = gmailClient.FetchMessage(info.Id);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to fetch message Id {info.Id}: {ex.Message}");
                                continue;
                            }

                            using (mailMessage)
                            {
                                // Extract plain text body
                                string plainBody = mailMessage.Body ?? string.Empty;

                                // Prepare JSON payload for Elasticsearch
                                string jsonPayload = $"{{ \"email\": \"{defaultEmail}\", \"messageId\": \"{info.Id}\", \"body\": \"{EscapeJson(plainBody)}\" }}";

                                // Build request URI
                                string requestUri = $"{elasticsearchUrl.TrimEnd('/')}/{indexName}/_doc";

                                // Send to Elasticsearch
                                try
                                {
                                    HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                                    HttpResponseMessage response = httpClient.PostAsync(requestUri, content).Result;
                                    if (!response.IsSuccessStatusCode)
                                    {
                                        Console.Error.WriteLine($"Elasticsearch indexing failed for message Id {info.Id}: {response.StatusCode}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Error sending message Id {info.Id} to Elasticsearch: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Simple JSON string escaper for line breaks and quotes
        private static string EscapeJson(string value)
        {
            if (value == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
