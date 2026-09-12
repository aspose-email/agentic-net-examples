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
            // Input HTML email file path
            string htmlPath = "email.html";

            // Verify input file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Input file '{htmlPath}' not found.");
                return;
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read '{htmlPath}': {ex.Message}");
                return;
            }

            // Create a MailMessage and set its HTML body
            using (MailMessage message = new MailMessage())
            {
                message.HtmlBody = htmlContent;
                message.Subject = Path.GetFileNameWithoutExtension(htmlPath);

                // Save the email to an MHTML stream using Aspose.Email
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    try
                    {
                        message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert email to MHTML: {ex.Message}");
                        return;
                    }

                    // Load the MHTML into Aspose.Words Document
                    Document doc;
                    try
                    {
                        doc = new Document(mhtmlStream);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load MHTML into Word document: {ex.Message}");
                        return;
                    }

                    // Set PDF metadata using Word document properties
                    doc.BuiltInDocumentProperties.Author = "Author Name";
                    doc.BuiltInDocumentProperties.Title = "Document Title";
                    doc.BuiltInDocumentProperties.Subject = "Document Subject";

                    // Output PDF file path
                    string pdfPath = $"{htmlPath}.pdf";

                    // Ensure output directory exists
                    try
                    {
                        string outputDir = Path.GetDirectoryName(pdfPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }

                    // Save the document as PDF with Aspose.Words
                    try
                    {
                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        doc.Save(pdfPath, pdfOptions);
                        Console.WriteLine($"PDF saved to '{pdfPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
