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
            // Define input and output paths
            string inputHtmlPath = "input.html";
            string outputPdfPath = "output.pdf";

            // Ensure input HTML file exists; create a minimal placeholder if missing
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

                string inputDir = Path.GetDirectoryName(inputHtmlPath);
                if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                {
                    Directory.CreateDirectory(inputDir);
                }

                File.WriteAllText(inputHtmlPath, "<html><body><p>Placeholder HTML content.</p></body></html>");
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the HTML file into a MailMessage using HtmlLoadOptions
            using (MailMessage message = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save the MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML stream into an Aspose.Words Document
                    Document doc = new Document(mhtmlStream);

                    // (Optional) Configure PDF save options, e.g., embed color profile if needed
                    Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                    // pdfOptions.ColorMode = PdfColorMode.Rgb; // Example setting; adjust as required

                    // Save the document as PDF
                    doc.Save(outputPdfPath, pdfOptions);
                }
            }

            Console.WriteLine("PDF generated successfully at: " + Path.GetFullPath(outputPdfPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
