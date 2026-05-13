using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Guard file existence
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

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the email message safely
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Validate From header
                bool fromValid = mailMessage.From != null && !string.IsNullOrWhiteSpace(mailMessage.From.Address);
                if (!fromValid)
                {
                    Console.WriteLine("Invalid or missing From header.");
                }

                // Validate To header
                bool toValid = mailMessage.To != null && mailMessage.To.Count > 0;
                if (toValid)
                {
                    foreach (MailAddress toAddress in mailMessage.To)
                    {
                        if (toAddress == null || string.IsNullOrWhiteSpace(toAddress.Address))
                        {
                            toValid = false;
                            break;
                        }
                    }
                }
                if (!toValid)
                {
                    Console.WriteLine("Invalid or missing To header.");
                }

                // Validate Subject header
                bool subjectValid = !string.IsNullOrWhiteSpace(mailMessage.Subject);
                if (!subjectValid)
                {
                    Console.WriteLine("Invalid or missing Subject header.");
                }

                // Overall result
                if (fromValid && toValid && subjectValid)
                {
                    Console.WriteLine("All required headers are present and correctly formatted.");
                }
                else
                {
                    Console.WriteLine("One or more required headers are missing or malformed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
