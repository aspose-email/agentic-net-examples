using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "email.html";
            string pdfPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder email content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            string outputDirectory = Path.GetDirectoryName(pdfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MailMessage email = MailMessage.Load(inputPath, new HtmlLoadOptions()))
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;
                Document doc = new Document(mhtmlStream);
                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
                {
                    Compliance = PdfCompliance.PdfA1b,
                    EmbedFullFonts = true
                };
                doc.Save(pdfPath, pdfOptions);
            }
            Console.WriteLine($"PDF/A fallback document saved to: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
