using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputPdfPath = "output.pdf";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputHtmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load HTML into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Prepare MHTML save options with custom CSS for print media
                MhtSaveOptions mhtOptions = new MhtSaveOptions
                {
                    CssStyles = "@media print { body { font-size: 12pt; } }"
                };

                // Save to MHTML in a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, mhtOptions);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream);
            {
                        // Save the document as PDF
                        doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                    }
                }
            }

            Console.WriteLine($"HTML successfully converted to PDF: {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
