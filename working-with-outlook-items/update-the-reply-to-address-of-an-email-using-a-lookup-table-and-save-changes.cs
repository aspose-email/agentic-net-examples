using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;


class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the email
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Lookup table for new Reply-To addresses keyed by original sender address
                var replyToLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "sender1@example.com", "replyto1@example.com" },
                    { "sender2@example.com", "replyto2@example.com" }
                };

                // Determine the key (use the From address)
                string fromAddress = message.From?.Address ?? string.Empty;

                if (replyToLookup.TryGetValue(fromAddress, out string newReplyTo))
                {
                    // Clear existing ReplyToList collection and set the new address
                    message.ReplyToList.Clear();
                    message.ReplyToList.Add(new MailAddress(newReplyTo));
                }
                else
                {
                    Console.Error.WriteLine($"No Reply-To mapping found for sender '{fromAddress}'. No changes applied.");
                }

                // Save the modified email using appropriate SaveOptions
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified email: {ex.Message}");
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
