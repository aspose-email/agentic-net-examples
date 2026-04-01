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
            // Define paths
            string inputHtmlPath = "input.html";
            string outputPdfPath = "output.pdf";
            string outputDocxPath = "output.docx";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputHtmlPath))
            {
                string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                File.WriteAllText(inputHtmlPath, placeholderHtml);
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the HTML file into a MailMessage using HtmlLoadOptions
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save the MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML stream into an Aspose.Words Document
                    Document document = new Document(mhtmlStream);
            {
                        // Export to PDF
                        document.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);

                        // Export to DOCX
                        document.Save(outputDocxPath, Aspose.Words.SaveFormat.Docx);
                    }
                }
            }

            Console.WriteLine("HTML conversion to PDF and DOCX completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
