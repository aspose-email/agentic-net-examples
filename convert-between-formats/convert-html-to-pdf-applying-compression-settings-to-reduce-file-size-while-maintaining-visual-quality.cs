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
            // Define input HTML and output PDF paths
            string inputHtmlPath = "input.html";
            string outputPdfPath = "output.pdf";

            // Ensure input HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputHtmlPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                    File.WriteAllText(inputHtmlPath, placeholderHtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load HTML into a MailMessage using HtmlLoadOptions
            MailMessage mailMessage;
            try
            {
                HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
                mailMessage = MailMessage.Load(inputHtmlPath, htmlLoadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {ex.Message}");
                return;
            }

            // Save the MailMessage to MHTML in a memory stream
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                try
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML to stream: {ex.Message}");
                    return;
                }

                // Load the MHTML into an Aspose.Words Document
                Document document;
                try
                {
                    document = new Document(mhtmlStream);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MHTML into Aspose.Words Document: {ex.Message}");
                    return;
                }

                // Configure PDF save options with compression settings
                Aspose.Words.Saving.PdfSaveOptions pdfSaveOptions = new Aspose.Words.Saving.PdfSaveOptions
                {
                    ImageCompression = PdfImageCompression.Jpeg,
                    JpegQuality = 80,
                    // Additional compression settings can be added here if needed
                };

                // Save the document as PDF with the specified options
                try
                {
                    document.Save(outputPdfPath, pdfSaveOptions);
                    Console.WriteLine($"HTML successfully converted to compressed PDF: {outputPdfPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save PDF file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
