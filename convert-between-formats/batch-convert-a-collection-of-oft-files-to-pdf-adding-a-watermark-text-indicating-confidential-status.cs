using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "InputOft";
            string outputFolder = "OutputPdf";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            Directory.CreateDirectory(outputFolder);

            string[] oftFiles;
            try
            {
                oftFiles = Directory.GetFiles(inputFolder, "*.oft");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate OFT files: {ex.Message}");
                return;
            }

            if (oftFiles.Length == 0)
            {
                Console.Error.WriteLine("No OFT files were found.");
                return;
            }

            foreach (string oftPath in oftFiles)
            {
                string tempMhtml = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(oftPath) + ".mhtml");
                try
                {
                    using (MapiMessage mapiMessage = MapiMessage.Load(oftPath))
                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                    {
                        mailMessage.Save(tempMhtml, Aspose.Email.SaveOptions.DefaultMhtml);
                    }

                    Document document = new Document(tempMhtml);
                    AddWatermark(document, "CONFIDENTIAL");

                    Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
                    {
                        Compliance = PdfCompliance.PdfA1b,
                        EmbedFullFonts = true
                    };

                    string pdfPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(oftPath) + ".pdf");
                    document.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"Converted: {oftPath} -> {pdfPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert '{oftPath}': {ex.Message}");
                }
                finally
                {
                    try
                    {
                        if (File.Exists(tempMhtml))
                        {
                            File.Delete(tempMhtml);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void AddWatermark(Document doc, string watermarkText)
    {
        foreach (Section section in doc.Sections)
        {
            HeaderFooter header = section.HeadersFooters[HeaderFooterType.HeaderPrimary];
            if (header == null)
            {
                header = new HeaderFooter(doc, HeaderFooterType.HeaderPrimary);
                section.HeadersFooters.Add(header);
            }

            Shape watermark = new Shape(doc, ShapeType.TextPlainText)
            {
                Width = 500,
                Height = 100,
                Rotation = -40,
                RelativeHorizontalPosition = RelativeHorizontalPosition.Page,
                RelativeVerticalPosition = RelativeVerticalPosition.Page,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WrapType = WrapType.None
            };
            watermark.TextPath.Text = watermarkText;
            watermark.TextPath.FontFamily = "Arial";
            watermark.TextPath.Size = 48;

            Paragraph paragraph = new Paragraph(doc);
            paragraph.AppendChild(watermark);
            header.AppendChild(paragraph);
        }
    }
}
