using System;
using System.IO;
using System.Collections.Generic;
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
            // Input and output directories (adjust as needed)
            string inputFolder = @"C:\InputMsg";
            string outputFolder = @"C:\OutputPdf";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists or create it
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            // Get all MSG files in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                // Guard against missing file
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                    {
                        // Convert to MailMessage (needed for MHTML export)
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                        {
                            // Export to MHTML in memory
                            using (MemoryStream mhtmlStream = new MemoryStream())
                            {
                                mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                                mhtmlStream.Position = 0;

                                // Load MHTML into Aspose.Words Document
                                Document doc = new Document(mhtmlStream);

                                // Placeholder: OCR processing of image attachments would occur here.
                                // Since Aspose.Email does not provide OCR, this step is omitted.
                                // If an OCR library were available, you would extract each image attachment,
                                // run OCR, and insert the recognized text into the document.

                                // Save the document as PDF
                                string pdfFileName = Path.GetFileNameWithoutExtension(msgPath) + ".pdf";
                                string pdfPath = Path.Combine(outputFolder, pdfFileName);
                                doc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                            }
                        }

                        // Optional: Process image attachments for OCR (not implemented)
                        foreach (MapiAttachment attachment in mapiMessage.Attachments)
                        {
                            // Simple check for common image extensions
                            string ext = Path.GetExtension(attachment.FileName)?.ToLowerInvariant();
                            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".tif" || ext == ".tiff")
                            {
                                // Attachment data can be accessed via attachment.Save(stream) if needed.
                                // OCR logic would be placed here.
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
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
