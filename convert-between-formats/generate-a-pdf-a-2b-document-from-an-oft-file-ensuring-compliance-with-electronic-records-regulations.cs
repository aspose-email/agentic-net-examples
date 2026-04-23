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
            string inputPath = "template.oft";
            string pdfPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Sample OFT Template",
                        "This is a placeholder OFT template."))
                    {
                        placeholder.SaveAsTemplate(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OFT: {ex.Message}");
                    return;
                }
            }

            string outputDirectory = Path.GetDirectoryName(pdfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage templateMessage = MapiMessage.Load(inputPath))
            using (MailMessage mail = templateMessage.ToMailMessage(new MailConversionOptions()))
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                mail.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
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
