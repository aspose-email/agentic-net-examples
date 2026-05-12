using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not.
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
                    using (MailMessage placeholder = new MailMessage("placeholder@example.com", "recipient@example.com", "Placeholder", "This is a placeholder email."))
                    {
                        placeholder.Save(inputPath);
                    }
                    Console.WriteLine($"Placeholder email created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                    return;
                }
                // No further processing needed for a newly created placeholder.
                return;
            }

            // Load the email message.
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email from '{inputPath}': {ex.Message}");
                return;
            }

            using (message)
            {
                // Track attachment names that have already been encountered.
                HashSet<string> seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                List<Attachment> duplicates = new List<Attachment>();

                foreach (Attachment attachment in message.Attachments)
                {
                    string name = attachment.Name ?? string.Empty;
                    if (seenNames.Contains(name))
                    {
                        duplicates.Add(attachment);
                    }
                    else
                    {
                        seenNames.Add(name);
                    }
                }

                // Remove duplicate attachments, keeping the first occurrence.
                foreach (Attachment duplicate in duplicates)
                {
                    message.Attachments.Remove(duplicate);
                }

                // Save the cleaned email.
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Email saved without duplicate attachments to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email to '{outputPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
