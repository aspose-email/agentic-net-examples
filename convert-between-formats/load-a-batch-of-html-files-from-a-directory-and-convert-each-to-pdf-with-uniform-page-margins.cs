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
            string inputDir = "InputHtml";
            string outputDir = "OutputPdf";

            // Verify input directory exists
            if (!Directory.Exists(inputDir))
            {
                Console.Error.WriteLine($"Input directory '{inputDir}' does not exist.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            string[] htmlFiles = Directory.GetFiles(inputDir, "*.html");
            foreach (string htmlPath in htmlFiles)
            {
                try
                {
                    // Load HTML as a MailMessage
                    MailMessage mailMessage = MailMessage.Load(htmlPath, new HtmlLoadOptions());

                    // Save to MHTML in memory
                    using (MemoryStream mhtmlStream = new MemoryStream())
                    {
                        mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;

                        // Load MHTML into Aspose.Words Document
                        Document doc = new Document(mhtmlStream);

                        // Set uniform page margins (1 inch = 72 points)
                        const double marginInPoints = 72.0;
                        foreach (Section section in doc.Sections)
                        {
                            section.PageSetup.LeftMargin = marginInPoints;
                            section.PageSetup.RightMargin = marginInPoints;
                            section.PageSetup.TopMargin = marginInPoints;
                            section.PageSetup.BottomMargin = marginInPoints;
                        }

                        // Determine PDF output path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(htmlPath);
                        string pdfPath = Path.Combine(outputDir, fileNameWithoutExt + ".pdf");

                        // Save as PDF
                        doc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                        Console.WriteLine($"Converted '{htmlPath}' to PDF successfully.");
                    }

                    mailMessage.Dispose();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{htmlPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
