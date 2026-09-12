using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            string htmlFilePath = "input.html";
            string pdfOutputPath = "output.pdf";
            string colorProfilePath = "color_profile.icc"; // Placeholder for ICC profile (not used in this example)

            // Ensure input HTML exists; create a minimal file if missing
            if (!File.Exists(htmlFilePath))
            {
                string minimalHtml = "<html><body><h1>Sample HTML</h1><p>This is a placeholder.</p></body></html>";
                File.WriteAllText(htmlFilePath, minimalHtml);
                Console.WriteLine($"Created placeholder HTML file at: {htmlFilePath}");
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load HTML into a MailMessage
            using (MailMessage message = new MailMessage())
            {
                message.HtmlBody = File.ReadAllText(htmlFilePath);
                message.Subject = "HTML to PDF Conversion";

                // Save the MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream, new Aspose.Words.Loading.LoadOptions());

                    // Prepare PDF save options (custom color profile not supported in this version)
                    Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();

                    // Save as PDF
                    doc.Save(pdfOutputPath, pdfOptions);
                }
            }

            Console.WriteLine($"PDF generated successfully at: {pdfOutputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
