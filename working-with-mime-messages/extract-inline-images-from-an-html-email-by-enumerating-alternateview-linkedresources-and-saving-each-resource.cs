using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source email file
            string emlPath = "EmailWithAttachEmbedded.eml";

            // Verify the input file exists
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

                Console.Error.WriteLine($"Input file '{emlPath}' does not exist.");
                return;
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Directory to store extracted images
                string outputDir = "ExtractedImages";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Iterate through each alternate view
                foreach (AlternateView view in message.AlternateViews)
                {
                    // Iterate through linked resources (inline images)
                    foreach (LinkedResource resource in view.LinkedResources)
                    {
                        // Determine a file name based on Content-Id or a GUID
                        string baseName = resource.ContentId;
                        if (string.IsNullOrEmpty(baseName))
                        {
                            baseName = Guid.NewGuid().ToString();
                        }

                        // Determine file extension from the content type
                        string extension = ".bin";
                        if (resource.ContentType != null && !string.IsNullOrEmpty(resource.ContentType.MediaType))
                        {
                            string mediaType = resource.ContentType.MediaType.ToLowerInvariant();
                            if (mediaType == "image/jpeg")
                                extension = ".jpg";
                            else if (mediaType == "image/png")
                                extension = ".png";
                            else if (mediaType == "image/gif")
                                extension = ".gif";
                        }

                        string outputPath = Path.Combine(outputDir, baseName + extension);

                        try
                        {
                            // Save the linked resource to a file
                            resource.Save(outputPath);
                            Console.WriteLine($"Saved inline image to '{outputPath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save resource '{baseName}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
