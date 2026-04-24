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
            // Input and output file paths
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
            string outputDirectory = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load HTML into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load MHTML into Aspose.Words Document
                    Document document = new Document(mhtmlStream);
            {
                        // Custom page size (A4) in points
                        const float a4Width = 595f;  // 210 mm
                        const float a4Height = 842f; // 297 mm

                        // Choose orientation
                        bool isLandscape = true; // Change to false for portrait

                        // Apply page setup
                        var pageSetup = document.Sections[0].PageSetup;
                        pageSetup.Orientation = isLandscape
                            ? Aspose.Words.Orientation.Landscape
                            : Aspose.Words.Orientation.Portrait;

                        if (isLandscape)
                        {
                            pageSetup.PageWidth = a4Height;
                            pageSetup.PageHeight = a4Width;
                        }
                        else
                        {
                            pageSetup.PageWidth = a4Width;
                            pageSetup.PageHeight = a4Height;
                        }

                        // Save the document as PDF
                        document.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                    }
                }
            }

            Console.WriteLine($"PDF generated successfully at: {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
