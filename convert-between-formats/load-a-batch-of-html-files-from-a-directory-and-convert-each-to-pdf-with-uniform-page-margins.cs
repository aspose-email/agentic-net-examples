using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputDirectory = "inputHtml";
            string outputDirectory = "outputPdf";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            string[] htmlFiles;
            try
            {
                htmlFiles = Directory.GetFiles(inputDirectory, "*.html");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Failed to enumerate HTML files: {fileEx.Message}");
                return;
            }

            foreach (string htmlFilePath in htmlFiles)
            {
                // Guard each file existence (redundant after GetFiles but required by rules)
                if (!File.Exists(htmlFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(htmlFilePath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {htmlFilePath}");
                    continue;
                }

                try
                {
                    // Load HTML into MailMessage
                    HtmlLoadOptions loadOptions = new HtmlLoadOptions();
                    using (MailMessage mailMessage = MailMessage.Load(htmlFilePath, loadOptions))
                    {
                        // Save MailMessage as MHTML into a memory stream
                        using (MemoryStream mhtmlStream = new MemoryStream())
                        {
                            mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                            mhtmlStream.Position = 0;

                            // Load MHTML into Aspose.Words Document
                            Document document = new Document(mhtmlStream);
            {
                                // Set uniform page margins (1 inch = 72 points)
                                document.FirstSection.PageSetup.LeftMargin = 72;
                                document.FirstSection.PageSetup.RightMargin = 72;
                                document.FirstSection.PageSetup.TopMargin = 72;
                                document.FirstSection.PageSetup.BottomMargin = 72;

                                // Determine PDF output path
                                string pdfFileName = Path.GetFileNameWithoutExtension(htmlFilePath) + ".pdf";
                                string pdfFilePath = Path.Combine(outputDirectory, pdfFileName);

                                // Save as PDF
                                document.Save(pdfFilePath, Aspose.Words.SaveFormat.Pdf);
                                Console.WriteLine($"Converted '{htmlFilePath}' to PDF successfully.");
                            }
                        }
                    }
                }
                catch (Exception conversionEx)
                {
                    Console.Error.WriteLine($"Error processing file '{htmlFilePath}': {conversionEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
