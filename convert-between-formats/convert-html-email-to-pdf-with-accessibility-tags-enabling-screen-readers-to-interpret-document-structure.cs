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

                Console.Error.WriteLine($"Input HTML file not found: {inputHtmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load HTML email into MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream);

                    // Configure PDF save options to include accessibility tags
                    Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
                    {
                        ExportDocumentStructure = true
                    };

                    // Save the document as PDF
                    doc.Save(outputPdfPath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
