using System;
using System.IO;
using System.Text;
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
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To = "receiver@example.com";
                        placeholder.Subject = "Placeholder Message";
                        placeholder.HtmlBody = "<html><body><p>Hello World</p><img src=\"cid:placeholder\"/></body></html>";

                        // Create a placeholder inline attachment
                        using (Attachment inlineAttachment = new Attachment(new MemoryStream(Encoding.UTF8.GetBytes("dummy")), "image/png"))
                        {
                            inlineAttachment.ContentId = "placeholder";
                            placeholder.Attachments.Add(inlineAttachment);
                        }

                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Load the existing message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message from '{inputPath}': {ex.Message}");
                return;
            }

            using (message)
            {
                // Ensure there is an HTML body to work with
                if (string.IsNullOrEmpty(message.HtmlBody))
                {
                    Console.Error.WriteLine("Message does not contain an HTML body.");
                    return;
                }

                string updatedHtml = message.HtmlBody;

                // Iterate over attachments and rename inline ones (those with a ContentId)
                foreach (Attachment attachment in message.Attachments)
                {
                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        string oldCid = attachment.ContentId;
                        string newCid = Guid.NewGuid().ToString();

                        // Update the attachment's ContentId
                        attachment.ContentId = newCid;

                        // Update all CID references in the HTML body
                        string oldReference = $"cid:{oldCid}";
                        string newReference = $"cid:{newCid}";
                        updatedHtml = updatedHtml.Replace(oldReference, newReference);
                    }
                }

                // Apply the updated HTML back to the message
                message.HtmlBody = updatedHtml;

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                        return;
                    }
                }

                // Save the modified message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with updated CID references to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message to '{outputPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
