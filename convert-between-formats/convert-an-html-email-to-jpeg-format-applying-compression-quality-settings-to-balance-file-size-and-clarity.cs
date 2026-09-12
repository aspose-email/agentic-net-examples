using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file containing the HTML email
            string inputPath = "input.eml";

            // Temporary MHTML file path
            string mhtmlPath = "temp.mhtml";

            // Output JPEG file path
            string outputPath = "output.jpg";

            // Ensure input file exists; create a placeholder if it does not
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
                        placeholder.Save(inputPath, Aspose.Email.SaveOptions.DefaultEml);
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

                Console.Error.WriteLine($"Input file '{inputPath}' not found. Placeholder created.");
                // Continue with the newly created placeholder
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the email as MHTML (required for visual export)
                message.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Load the MHTML into Aspose.Words Document
            Document doc = new Document(mhtmlPath);

            // Configure JPEG save options with desired quality (0-100)
            ImageSaveOptions jpegOptions = new ImageSaveOptions(SaveFormat.Jpeg)
            {
                // Set the compression quality (e.g., 80 for good balance)
                JpegQuality = 80,
                // Render the first page only (email is usually a single page)
                PageSet = new PageSet(0)
            };

            // Save the document as JPEG
            doc.Save(outputPath, jpegOptions);

            // Clean up temporary MHTML file
            try
            {
                if (File.Exists(mhtmlPath))
                {
                    File.Delete(mhtmlPath);
                }
            }
            catch (Exception cleanupEx)
            {
                Console.Error.WriteLine($"Cleanup warning: {cleanupEx.Message}");
            }

            Console.WriteLine($"Email successfully converted to JPEG: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
