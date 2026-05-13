using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input MIME message file (EML)
            string inputPath = "message.eml";

            // Verify the input file exists
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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Output directory for the split parts
            string outputDir = "output";

            // Ensure the output directory exists
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {dirEx.Message}");
                return;
            }

            // Load the multipart MIME message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Counter for naming parts
                int partIndex = 1;

                // Save each attachment as a separate file
                foreach (Attachment attachment in message.Attachments)
                {
                    string attachmentPath = Path.Combine(outputDir, $"attachment_{partIndex}_{attachment.Name}");
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment to '{attachmentPath}'.");
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {attEx.Message}");
                    }
                    partIndex++;
                }

                // Save each alternate view (e.g., text/plain, text/html) as a separate file
                foreach (AlternateView view in message.AlternateViews)
                {
                    string extension = view.ContentType.MediaType.Replace("/", "_"); // e.g., "text_plain"
                    string viewPath = Path.Combine(outputDir, $"view_{partIndex}_{extension}.dat");

                    try
                    {
                        using (Stream viewStream = view.ContentStream)
                        using (FileStream fileStream = new FileStream(viewPath, FileMode.Create, FileAccess.Write))
                        {
                            viewStream.CopyTo(fileStream);
                        }
                        Console.WriteLine($"Saved alternate view to '{viewPath}'.");
                    }
                    catch (Exception viewEx)
                    {
                        Console.Error.WriteLine($"Failed to save alternate view #{partIndex}: {viewEx.Message}");
                    }
                    partIndex++;
                }

                // If the message body itself is a plain text part (no attachments/alternate views), save it
                if (message.Attachments.Count == 0 && message.AlternateViews.Count == 0 && !string.IsNullOrEmpty(message.Body))
                {
                    string bodyPath = Path.Combine(outputDir, $"body_{partIndex}.txt");
                    try
                    {
                        File.WriteAllText(bodyPath, message.Body);
                        Console.WriteLine($"Saved message body to '{bodyPath}'.");
                    }
                    catch (Exception bodyEx)
                    {
                        Console.Error.WriteLine($"Failed to save message body: {bodyEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
