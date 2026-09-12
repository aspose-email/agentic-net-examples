using System;
using System.IO;
using Aspose.Words;

class Program
{
    static void Main()
    {
        // Paths for input HTML email and output PDF
        string htmlPath = "email.html";   // TODO: replace with actual HTML file path
        string pdfPath = "output.pdf";    // TODO: replace with desired PDF output path

        // Verify input file exists
        if (!File.Exists(htmlPath))
        {
            Console.Error.WriteLine($"Input HTML file not found: {htmlPath}");
            return;
        }

        // Ensure output directory exists
        string outputDirectory = Path.GetDirectoryName(pdfPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load the HTML content into a Word document
            Document document = new Document(htmlPath);

            // Insert a Table of Contents at the beginning of the document
            DocumentBuilder builder = new DocumentBuilder(document);
            builder.MoveToDocumentStart();
            builder.InsertTableOfContents("\\o \"1-3\" \\h \\z \\u");

            // Save the document as PDF
            document.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);

            Console.WriteLine($"PDF successfully created at: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}
