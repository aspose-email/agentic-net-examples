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
            string htmlPath = "input.html";
            string pdfPath = "output.pdf";

            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><title>Sample</title></head><body><p>Sample HTML content.</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            string outputDir = Path.GetDirectoryName(Path.GetFullPath(pdfPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MailMessage mail = MailMessage.Load(htmlPath, new HtmlLoadOptions()))
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                mail.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;

                Document doc = new Document(mhtmlStream);
                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions()
                {
                    // EmbedFullFonts is supported; EmbedStandardWindowsFonts is not available in recent Aspose.Words versions.
                    EmbedFullFonts = true
                };
                doc.Save(pdfPath, pdfOptions);
            }

            Console.WriteLine($"HTML successfully converted to PDF with embedded fonts: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
