using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Words;
using Aspose.Words.Saving;

namespace ConvertEmlBatchToPdf
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputDirectory = "InputEml";
                string outputDirectory = "OutputPdf";

                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                    return;
                }

                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                string[] emlFiles;
                try
                {
                    emlFiles = Directory.GetFiles(inputDirectory, "*.eml");
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Error accessing files in '{inputDirectory}': {fileEx.Message}");
                    return;
                }

                foreach (string emlPath in emlFiles)
                {
                    try
                    {
                        // Load the EML file with explicit Aspose.Email load options
                        Aspose.Email.EmlLoadOptions loadOptions = new Aspose.Email.EmlLoadOptions();
                        using (MailMessage message = MailMessage.Load(emlPath, loadOptions))
                        {
                            // Save the message as MHTML into a memory stream
                            using (MemoryStream mhtmlStream = new MemoryStream())
                            {
                                message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                                mhtmlStream.Position = 0;

                                // Load the MHTML into Aspose.Words Document
                                Document document = new Document(mhtmlStream);

                                // Determine PDF output path
                                string pdfFileName = Path.GetFileNameWithoutExtension(emlPath) + ".pdf";
                                string pdfPath = Path.Combine(outputDirectory, pdfFileName);

                                // Save as PDF
                                document.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                            }
                        }

                        Console.WriteLine($"Converted '{emlPath}' to PDF successfully.");
                    }
                    catch (Exception convertEx)
                    {
                        Console.Error.WriteLine($"Failed to convert '{emlPath}': {convertEx.Message}");
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
