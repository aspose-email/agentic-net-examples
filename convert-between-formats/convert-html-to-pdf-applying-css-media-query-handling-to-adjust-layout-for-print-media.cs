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
            // Input HTML file path
            string inputPath = "input.html";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath,
                        "<html><head><style>@media print { body { font-size: 12pt; } }</style></head><body><p>Hello, world!</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Prepare the mail message with HTML body
            using (MailMessage message = new MailMessage())
            {
                message.HtmlBody = htmlContent;

                // Save the message as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    try
                    {
                        message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message as MHTML: {ex.Message}");
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
                        Console.Error.WriteLine($"Failed to load MHTML into Aspose.Words: {ex.Message}");
                        return;
                    }

                    // Output PDF file path
                    string outputPath = "output.pdf";

                    // Ensure the output directory exists
                    try
                    {
                        string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                        return;
                    }

                    // Save the document as PDF
                    try
                    {
                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        doc.Save(outputPath, pdfOptions);
                        Console.WriteLine($"PDF generated successfully at '{outputPath}'.");
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
