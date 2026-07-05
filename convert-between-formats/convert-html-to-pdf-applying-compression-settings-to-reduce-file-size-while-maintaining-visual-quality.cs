using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        const string htmlPath = "input.html";
        const string pdfPath = "output.pdf";

        // Verify input HTML file exists
        if (!File.Exists(htmlPath))
        {
            Console.Error.WriteLine($"Input file not found: {htmlPath}");
            return;
        }

        try
        {
            // Load HTML content
            string htmlContent = File.ReadAllText(htmlPath);

            // Create a MailMessage with the HTML body
            using (MailMessage mail = new MailMessage())
            {
                mail.HtmlBody = htmlContent;

                // Save the message as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mail.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load the MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream);
            {
                        // Configure PDF save options for compression
                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
                        {
                            ImageCompression = PdfImageCompression.Jpeg,
                            JpegQuality = 90,
                            OptimizeOutput = true
                        };

                        // Save the document as PDF with the specified options
                        doc.Save(pdfPath, pdfOptions);
                    }
                }
            }

            Console.WriteLine($"HTML successfully converted to compressed PDF: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
