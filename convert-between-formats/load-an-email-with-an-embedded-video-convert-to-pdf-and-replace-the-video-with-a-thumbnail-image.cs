using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            // Paths (replace with actual paths as needed)
            string inputEmailPath = "email_with_video.eml";
            string thumbnailImagePath = "thumbnail.jpg";
            string outputPdfPath = "output.pdf";

            // Verify input files exist
            if (!File.Exists(inputEmailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputEmailPath, Aspose.Email.SaveOptions.DefaultEml);
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

                Console.Error.WriteLine($"Input email file not found: {inputEmailPath}");
                return;
            }

            if (!File.Exists(thumbnailImagePath))
            {
                Console.Error.WriteLine($"Thumbnail image file not found: {thumbnailImagePath}");
                return;
            }

            // Load the email message
            MailMessage mailMessage = MailMessage.Load(inputEmailPath);

            // Replace embedded video resources with the thumbnail image
            LinkedResourceCollection resources = mailMessage.LinkedResources;
            for (int i = 0; i < resources.Count; i++)
            {
                LinkedResource resource = resources[i];
                if (resource.ContentType.MediaType.StartsWith("video", StringComparison.OrdinalIgnoreCase))
                {
                    // Create a new linked resource for the thumbnail image
                    LinkedResource thumbnailResource = new LinkedResource(
                        thumbnailImagePath,
                        Aspose.Email.Mime.MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = resource.ContentId
                    };

                    // Replace the video resource with the thumbnail
                    resources.RemoveAt(i);
                    resources.Insert(i, thumbnailResource);
                }
            }

            // Save the modified email to MHTML in memory
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;

                // Load MHTML into Aspose.Words Document
                Document doc = new Document(mhtmlStream);

                // Save the document as PDF
                doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
            }

            Console.WriteLine($"PDF saved successfully to: {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
