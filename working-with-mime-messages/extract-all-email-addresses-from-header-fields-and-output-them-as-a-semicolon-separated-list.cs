using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholderMsg = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholderMsg.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the email message.
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                List<string> addressList = new List<string>();
                string[] headerNames = { "From", "Sender", "To", "Cc", "Bcc", "Reply-To" };
                string emailPattern = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}";

                foreach (string header in headerNames)
                {
                    string headerValue = message.Headers[header];
                    if (string.IsNullOrEmpty(headerValue))
                        continue;

                    foreach (Match match in Regex.Matches(headerValue, emailPattern, RegexOptions.IgnoreCase))
                    {
                        addressList.Add(match.Value);
                    }
                }

                // Output as semicolon‑separated list.
                Console.WriteLine(string.Join(";", addressList));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
