using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        // Sample HTML content
        string htmlContent = "<html><body><h1>Sample PDF from HTML</h1><p>This is a paragraph.</p></body></html>";

        // Create a MailMessage and set its HTML body
        MailMessage message = new MailMessage();
        message.HtmlBody = htmlContent;

        // Desired page size (A4) in points (1 point = 1/72 inch)
        // Width = 595, Height = 842 for portrait
        double pageWidth = 595;
        double pageHeight = 842;

        // Set orientation: true for landscape, false for portrait
        bool landscape = true;
        if (landscape)
        {
            // Swap dimensions for landscape
            double temp = pageWidth;
            pageWidth = pageHeight;
            pageHeight = temp;
        }

        // Output PDF path
        string outputPath = Path.Combine("output", "sample.pdf");

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Convert MailMessage to MHTML in memory
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;

                // Load MHTML into Aspose.Words Document
                Document doc = new Document(mhtmlStream, new Aspose.Words.Loading.LoadOptions());

                // Apply custom page size
                Section section = doc.FirstSection;
                PageSetup pageSetup = section.PageSetup;
                pageSetup.PageWidth = (float)pageWidth;
                pageSetup.PageHeight = (float)pageHeight;
                pageSetup.Orientation = landscape ? Orientation.Landscape : Orientation.Portrait;

                // Save as PDF
                doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
            }

            Console.WriteLine($"PDF generated successfully at: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error generating PDF: {ex.Message}");
        }
    }
}
