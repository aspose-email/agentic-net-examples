using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    // DTO matching the JSON structure for deserialization
    private class MailMessageDto
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    static void Main()
    {
        try
        {
            string jsonPath = "message.json";
            string outputPath = "reconstructed.eml";

            // Ensure input JSON file exists
            if (!File.Exists(jsonPath))
            {
                // Create a minimal placeholder JSON
                var placeholder = new MailMessageDto
                {
                    From = "sender@example.com",
                    To = new List<string> { "recipient@example.com" },
                    CC = new List<string>(),
                    Bcc = new List<string>(),
                    Subject = "Placeholder Subject",
                    Body = "Placeholder body."
                };
                string placeholderJson = JsonSerializer.Serialize(placeholder, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, placeholderJson);
                Console.Error.WriteLine($"Input JSON not found. Created placeholder at '{jsonPath}'.");
                return;
            }

            // Read and deserialize JSON
            MailMessageDto dto;
            using (FileStream fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                dto = JsonSerializer.Deserialize<MailMessageDto>(fs);
            }

            if (dto == null)
            {
                Console.Error.WriteLine("Failed to deserialize JSON.");
                return;
            }

            // Construct MailMessage from DTO
            using (MailMessage mailMessage = new MailMessage())
            {
                // From address
                if (!string.IsNullOrEmpty(dto.From))
                {
                    mailMessage.From = new MailAddress(dto.From);
                }

                // To recipients
                if (dto.To != null)
                {
                    foreach (string addr in dto.To)
                    {
                        if (!string.IsNullOrWhiteSpace(addr))
                            mailMessage.To.Add(new MailAddress(addr));
                    }
                }

                // CC recipients
                if (dto.CC != null)
                {
                    foreach (string addr in dto.CC)
                    {
                        if (!string.IsNullOrWhiteSpace(addr))
                            mailMessage.CC.Add(new MailAddress(addr));
                    }
                }

                // BCC recipients
                if (dto.Bcc != null)
                {
                    foreach (string addr in dto.Bcc)
                    {
                        if (!string.IsNullOrWhiteSpace(addr))
                            mailMessage.Bcc.Add(new MailAddress(addr));
                    }
                }

                // Subject and Body
                mailMessage.Subject = dto.Subject ?? string.Empty;
                mailMessage.Body = dto.Body ?? string.Empty;

                // Save reconstructed MIME content to file using EmlSaveOptions
                var saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                mailMessage.Save(outputPath, saveOptions);
                Console.WriteLine($"MailMessage reconstructed and saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
