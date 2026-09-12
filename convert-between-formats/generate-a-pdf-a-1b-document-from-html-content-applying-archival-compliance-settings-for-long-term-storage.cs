using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        // Input and output file paths
        const string htmlPath = "sample.html";
        const string mhtmlPath = "sample.mhtml";
        const string pdfPath = "output.pdf";

        // Ensure HTML source exists; create a minimal placeholder if missing
        if (!File.Exists(htmlPath))
        {
            try
            {
                File.WriteAllText(htmlPath, "<html><body><h1>Sample Document</h1><p>This is a test.</p></body></html>");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load HTML content into a MailMessage
            string htmlContent = File.ReadAllText(htmlPath);
            using (MailMessage mail = new MailMessage())
            {
                mail.HtmlBody = htmlContent;

                // Save the MailMessage as MHTML using the default MHTML options
                mail.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Load the generated MHTML into Aspose.Words Document
            Document doc = new Document(mhtmlPath);

            // Configure PDF/A-1b compliance options
            Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
            {
                Compliance = PdfCompliance.PdfA1b,
                // Embed all fonts to satisfy PDF/A requirements
                EmbedFullFonts = true,
                // Export document structure for better accessibility
                ExportDocumentStructure = true
            };

            // Save the document as PDF/A-1b
            doc.Save(pdfPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            return;
        }

        Console.WriteLine($"PDF/A-1b document created successfully at '{pdfPath}'.");
    }
}
