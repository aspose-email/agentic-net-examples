using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailJsonExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
                string username = "username";
                string password = "password";
                string messageUri = "/mail/inbox/12345";
                string outputPath = "message.json";

                // Guard against placeholder credentials
                if (exchangeUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Ensure output directory exists
                try
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                    return;
                }

                // Create and use Exchange client
                try
                {
                    using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                    {
                        // Fetch the mail message
                        MailMessage mailMessage;
                        try
                        {
                            mailMessage = client.FetchMessage(messageUri);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message: {fetchEx.Message}");
                            return;
                        }

                        // Prepare DTO for JSON serialization
                        MailMessageDto dto = new MailMessageDto();
                        dto.Subject = mailMessage.Subject ?? string.Empty;
                        dto.From = mailMessage.From != null ? mailMessage.From.Address : string.Empty;

                        dto.To = new List<string>();
                        foreach (MailAddress address in mailMessage.To)
                        {
                            dto.To.Add(address.Address);
                        }

                        dto.Body = mailMessage.Body != null ? mailMessage.Body : string.Empty;

                        // Serialize to JSON
                        string json;
                        try
                        {
                            json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
                        }
                        catch (Exception jsonEx)
                        {
                            Console.Error.WriteLine($"JSON serialization failed: {jsonEx.Message}");
                            return;
                        }

                        // Write JSON to file
                        try
                        {
                            File.WriteAllText(outputPath, json);
                            Console.WriteLine($"Message serialized to JSON and saved at: {outputPath}");
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write JSON file: {writeEx.Message}");
                        }

                        // Dispose mailMessage if needed
                        if (mailMessage is IDisposable disposableMessage)
                        {
                            disposableMessage.Dispose();
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    // DTO class for JSON representation of MailMessage
    public class MailMessageDto
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Body { get; set; }
    }
}
