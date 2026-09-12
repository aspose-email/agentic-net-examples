using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

namespace ConvertMsgToPdfWithOcr
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories
                string inputDir = Path.Combine(Directory.GetCurrentDirectory(), "InputMsgs");
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "OutputPdfs");

                // Ensure input directory exists
                if (!Directory.Exists(inputDir))
                {
                    Console.Error.WriteLine($"Input directory not found: {inputDir}");
                    return;
                }

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Process each MSG file in the input directory
                foreach (string msgPath in Directory.GetFiles(inputDir, "*.msg"))
                {
                    try
                    {
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(msgPath);
                        string tempMhtmlPath = Path.Combine(Path.GetTempPath(), fileNameWithoutExt + ".mhtml");
                        string pdfPath = Path.Combine(outputDir, fileNameWithoutExt + ".pdf");

                        // Load the MSG file
                        MapiMessage mapMsg = MapiMessage.Load(msgPath);

                        // Convert to MailMessage and save as MHTML (visual representation)
                        using (MailMessage mailMessage = mapMsg.ToMailMessage(new MailConversionOptions()))
                        {
                            mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
                        }

                        // Load the MHTML into Aspose.Words Document
                        Document doc = new Document(tempMhtmlPath);

                        // Prepare PDF save options (OCR settings are not available in this version)
                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();

                        // Save as PDF
                        doc.Save(pdfPath, pdfOptions);

                        // Clean up temporary MHTML file
                        if (File.Exists(tempMhtmlPath))
                        {
                            File.Delete(tempMhtmlPath);
                        }

                        Console.WriteLine($"Converted '{msgPath}' to PDF successfully.");
                    }
                    catch (Exception exFile)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgPath}': {exFile.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
