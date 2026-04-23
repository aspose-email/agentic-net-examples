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
            // Define paths
            string inputFilePath = "input.eml";
            string thumbnailFilePath = "thumbnail.jpg";
            string outputDirectory = "output";
            string mhtmlFilePath = Path.Combine(outputDirectory, "temp.mhtml");
            string pdfFilePath = Path.Combine(outputDirectory, "result.pdf");

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Verify input email file exists
            if (!File.Exists(inputFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFilePath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputFilePath))
            {
                // Replace embedded video attachments with a thumbnail image
                for (int i = mailMessage.Attachments.Count - 1; i >= 0; i--)
                {
                    Attachment attachment = mailMessage.Attachments[i];
                    string mediaType = attachment.ContentType.MediaType ?? string.Empty;
                    if (mediaType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                    {
                        // Remove the video attachment
                        mailMessage.Attachments.RemoveAt(i);

                        // Add thumbnail image if it exists
                        if (File.Exists(thumbnailFilePath))
                        {
                            using (FileStream thumbStream = File.OpenRead(thumbnailFilePath))
                            {
                                Attachment thumbnail = new Attachment(thumbStream, "thumbnail.jpg", MediaTypeNames.Image.Jpeg);
                                mailMessage.Attachments.Add(thumbnail);
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine($"Thumbnail image not found: {thumbnailFilePath}");
                        }
                    }
                }

                // Save the modified message as MHTML (intermediate format for rendering)
                mailMessage.Save(mhtmlFilePath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Load the MHTML into Aspose.Words Document
            Document doc = new Document(mhtmlFilePath);
            {
                // Save the document as PDF
                doc.Save(pdfFilePath, Aspose.Words.SaveFormat.Pdf);
            }

            Console.WriteLine($"PDF generated successfully at: {pdfFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
