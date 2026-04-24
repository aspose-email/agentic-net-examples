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
            // Define input and output directories
            string inputDirectory = "InputMsgs";
            string outputDirectory = "OutputPdfs";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist. No files to process.");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                return;
            }

            // Get all MSG files in the input directory
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Error accessing files in '{inputDirectory}': {fileEx.Message}");
                return;
            }

            if (msgFiles.Length == 0)
            {
                Console.Error.WriteLine($"No MSG files found in '{inputDirectory}'.");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                try
                {
                    // Load the MSG file
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                    {
                        // Convert to MailMessage (needed for MHTML export)
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                        {
                            // Save to a temporary MHTML file
                            string tempMhtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mhtml");
                            try
                            {
                                mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save MHTML for '{msgFilePath}': {saveEx.Message}");
                                continue;
                            }

                            // Load the MHTML into Aspose.Words Document
                            Document wordDocument = new Document(tempMhtmlPath);
                            DocumentBuilder builder = new DocumentBuilder(wordDocument);

                            // Insert a heading for attachments
                            builder.Writeln();
                            builder.Font.Size = 14;
                            builder.Font.Bold = true;
                            builder.Writeln("Attachments:");

                            // Process each attachment
                            foreach (MapiAttachment attachment in mapiMessage.Attachments)
                            {
                                // Save attachment to a temporary file
                                string tempAttachmentPath = Path.Combine(Path.GetTempPath(), attachment.FileName);
                                try
                                {
                                    attachment.Save(tempAttachmentPath);
                                }
                                catch (Exception attSaveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}' from '{msgFilePath}': {attSaveEx.Message}");
                                    continue;
                                }

                                // Insert a hyperlink to the attachment file
                                builder.Font.Size = 12;
                                builder.Font.Bold = false;
                                // InsertHyperlink(string url, string text, bool isBookmark)
                                builder.InsertHyperlink(tempAttachmentPath, attachment.FileName, false);
                                builder.Writeln();

                                // Clean up the temporary attachment file
                                try
                                {
                                    File.Delete(tempAttachmentPath);
                                }
                                catch
                                {
                                    // Ignored – non‑critical cleanup
                                }
                            }

                            // Save the final PDF
                            string pdfFileName = Path.GetFileNameWithoutExtension(msgFilePath) + ".pdf";
                            string pdfOutputPath = Path.Combine(outputDirectory, pdfFileName);
                            try
                            {
                                wordDocument.Save(pdfOutputPath, Aspose.Words.SaveFormat.Pdf);
                                Console.WriteLine($"Converted '{msgFilePath}' to PDF with attachments: '{pdfOutputPath}'.");
                            }
                            catch (Exception pdfEx)
                            {
                                Console.Error.WriteLine($"Failed to save PDF for '{msgFilePath}': {pdfEx.Message}");
                            }

                            // Delete the temporary MHTML file
                            try
                            {
                                File.Delete(tempMhtmlPath);
                            }
                            catch
                            {
                                // Ignored – non‑critical cleanup
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unexpected error: {outerEx.Message}");
        }
    }
}
