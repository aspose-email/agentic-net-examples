using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output directories (adjust as needed)
            string inputDirectory = "InputMsg";
            string outputDirectory = "OutputPdf";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Get all .msg files in the input directory
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {fileEx.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                // Guard against missing file (should not happen after enumeration)
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {msgFilePath}");
                    continue;
                }

                try
                {
                    // Load MSG as MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                    {
                        // Convert to MailMessage
                        MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                        // Save to temporary MHTML in memory
                        using (MemoryStream mhtmlStream = new MemoryStream())
                        {
                            mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                            mhtmlStream.Position = 0;

                            // Load MHTML into Aspose.Words Document
                            Document wordDoc = new Document(mhtmlStream);

                            // Apply custom page margins (1 inch = 72 points)
                            foreach (Section section in wordDoc.Sections)
                            {
                                PageSetup pageSetup = section.PageSetup;
                                pageSetup.TopMargin = 72;
                                pageSetup.BottomMargin = 72;
                                pageSetup.LeftMargin = 72;
                                pageSetup.RightMargin = 72;
                            }

                            // Determine output PDF path
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(msgFilePath);
                            string pdfPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                            // Save PDF
                            wordDoc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                            Console.WriteLine($"Converted '{msgFilePath}' to PDF successfully.");
                        }

                        // Dispose MailMessage
                        mailMessage.Dispose();
                    }
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Failed to convert '{msgFilePath}': {convEx.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
