using System;
using System.Net.Http;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Sender and recipient details
            string fromAddress = "sender@example.com";
            string toAddress = "recipient@example.org";

            // Extract domain from the sender address
            string[] parts = fromAddress.Split('@');
            if (parts.Length != 2)
            {
                Console.Error.WriteLine("Invalid sender email address.");
                return;
            }
            string domain = parts[1];

            // Guard: skip external SPF validation for placeholder domains
            if (!IsPlaceholderDomain(domain))
            {
                // Validate SPF record for the domain
                if (!HasSpfRecord(domain))
                {
                    Console.Error.WriteLine($"No SPF record found for domain '{domain}'. Aborting send.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Placeholder domain detected – skipping SPF validation.");
            }

            // Create a simple mail message
            MailMessage message = new MailMessage
            {
                From = fromAddress,
                Subject = "Test Email with SPF Validation",
                Body = "This email was sent after confirming SPF record existence."
            };
            message.To.Add(toAddress);

            // SMTP client configuration (placeholder values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Guard: skip actual SMTP operation for placeholder credentials
            if (IsPlaceholderSmtp(smtpHost, smtpUser, smtpPass))
            {
                Console.WriteLine("Placeholder SMTP configuration detected – skipping actual send.");
                Console.WriteLine("Message prepared successfully (not sent).");
                return;
            }

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials before sending
                    if (!client.ValidateCredentials())
                    {
                        Console.Error.WriteLine("SMTP credentials are invalid.");
                        return;
                    }

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Checks whether the given domain has an SPF TXT record
    private static bool HasSpfRecord(string domain)
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Use Google's DNS-over-HTTPS service to query TXT records
                string requestUri = $"https://dns.google/resolve?name={domain}&type=TXT";
                HttpResponseMessage response = httpClient.GetAsync(requestUri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"Failed to query DNS for domain '{domain}'.");
                    return false;
                }

                string json = response.Content.ReadAsStringAsync().Result;
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("Answer", out JsonElement answers))
                    {
                        foreach (JsonElement answer in answers.EnumerateArray())
                        {
                            if (answer.TryGetProperty("data", out JsonElement dataElement))
                            {
                                string txt = dataElement.GetString();
                                if (!string.IsNullOrEmpty(txt) && txt.Contains("v=spf1"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error checking SPF record: {ex.Message}");
        }

        return false;
    }

    // Determines if the domain is a placeholder (e.g., example.com)
    private static bool IsPlaceholderDomain(string domain)
    {
        return string.Equals(domain, "example.com", StringComparison.OrdinalIgnoreCase);
    }

    // Determines if the SMTP configuration uses placeholder values
    private static bool IsPlaceholderSmtp(string host, string user, string pass)
    {
        return host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
               user.Equals("username", StringComparison.OrdinalIgnoreCase) ||
               pass.Equals("password", StringComparison.OrdinalIgnoreCase);
    }
}
