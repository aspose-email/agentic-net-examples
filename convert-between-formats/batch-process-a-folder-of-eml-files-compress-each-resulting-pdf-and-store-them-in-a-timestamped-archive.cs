using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string emlFolderPath = "InputEmls";
            string tempFolderPath = "TempProcessing";
            string outputFolderPath = "OutputArchives";

            // Verify input folder exists
            if (!Directory.Exists(emlFolderPath))
            {
                Console.Error.WriteLine($"Input folder does not exist: {emlFolderPath}");
                return;
            }

            // Ensure temporary and output folders exist
            Directory.CreateDirectory(tempFolderPath);
            Directory.CreateDirectory(outputFolderPath);

            // Create timestamped archive name
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string archivePath = Path.Combine(outputFolderPath, $"Archive_{timestamp}.zip");

            // Create zip archive
            using (FileStream zipStream = new FileStream(archivePath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
            {
                // Process each EML file
                foreach (string emlFilePath in Directory.GetFiles(emlFolderPath, "*.eml"))
                {
                    try
                    {
                        // Load EML message
                        using (MailMessage message = MailMessage.Load(emlFilePath))
                        {
                            // Prepare temporary HTML and PDF paths
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(emlFilePath);
                            string htmlPath = Path.Combine(tempFolderPath, $"{fileNameWithoutExt}.html");
                            string pdfPath = Path.Combine(tempFolderPath, $"{fileNameWithoutExt}.pdf");

                            // Save as HTML with embedded resources
                            Aspose.Email.HtmlSaveOptions htmlOptions = new Aspose.Email.HtmlSaveOptions
                            {
                                ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                            };
                            message.Save(htmlPath, htmlOptions);

                            // Convert HTML to PDF using Aspose.Words
                            Document doc = new Document(htmlPath);
            {
                                doc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                            }

                            // Add PDF to zip archive
                            archive.CreateEntryFromFile(pdfPath, Path.GetFileName(pdfPath), System.IO.Compression.CompressionLevel.Optimal);

                            // Clean up temporary files
                            File.Delete(htmlPath);
                            File.Delete(pdfPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{emlFilePath}': {ex.Message}");
                        // Continue with next file
                    }
                }
            }

            // Clean up temporary folder if empty
            try
            {
                if (Directory.GetFiles(tempFolderPath).Length == 0)
                {
                    Directory.Delete(tempFolderPath, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }

            Console.WriteLine($"Archive created at: {archivePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
