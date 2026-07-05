using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;

namespace BatchEmlToPdfArchive
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Author note: This sample processes EML files, converts them to PDF, and archives them.
                string inputFolder = "InputEml";
                string tempPdfFolder = Path.Combine(Path.GetTempPath(), "EmlPdfTemp");
                string outputFolder = "OutputArchive";

                // Verify input folder exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                    return;
                }

                // Ensure temporary and output directories exist
                Directory.CreateDirectory(tempPdfFolder);
                Directory.CreateDirectory(outputFolder);

                // Process each EML file in the input folder
                foreach (string emlPath in Directory.GetFiles(inputFolder, "*.eml"))
                {
                    try
                    {
                        // Load the EML message with desired options
                        EmlLoadOptions loadOptions = new EmlLoadOptions()
                        {
                            PreserveEmbeddedMessageFormat = true,
                            PreserveTnefAttachments = true
                        };

                        using (MailMessage message = MailMessage.Load(emlPath, loadOptions))
                        {
                            // Determine PDF output path
                            string pdfFileName = Path.GetFileNameWithoutExtension(emlPath) + ".pdf";
                            string pdfPath = Path.Combine(tempPdfFolder, pdfFileName);

                            // Save the message as PDF (extension infers format)
                            message.Save(pdfPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert '{emlPath}': {ex.Message}");
                    }
                }

                // Create a timestamped ZIP archive of the generated PDFs
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string zipPath = Path.Combine(outputFolder, $"Archive_{timestamp}.zip");

                try
                {
                    using (FileStream zipStream = new FileStream(zipPath, FileMode.Create))
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                    {
                        foreach (string pdfFile in Directory.GetFiles(tempPdfFolder, "*.pdf"))
                        {
                            string entryName = Path.GetFileName(pdfFile);
                            archive.CreateEntryFromFile(pdfFile, entryName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create archive: {ex.Message}");
                    return;
                }

                // Clean up temporary PDF files
                try
                {
                    Directory.Delete(tempPdfFolder, true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to clean temporary files: {ex.Message}");
                }

                Console.WriteLine($"Archive created successfully at: {zipPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
