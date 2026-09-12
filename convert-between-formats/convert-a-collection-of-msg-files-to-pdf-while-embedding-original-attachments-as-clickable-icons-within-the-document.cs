using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

namespace ConvertMsgToPdf
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputDirectory = "InputMsgs";
                string outputDirectory = "OutputPdfs";
                string attachmentsDirectory = "Attachments";

                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist. Creating it.");
                    Directory.CreateDirectory(inputDirectory);
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                if (!Directory.Exists(attachmentsDirectory))
                {
                    Directory.CreateDirectory(attachmentsDirectory);
                }

                string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
                foreach (string msgFilePath in msgFiles)
                {
                    try
                    {
                        MapiMessage mapMsg = MapiMessage.Load(msgFilePath);

                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mailMessage = mapMsg.ToMailMessage(conversionOptions))
                        {
                            string tempMhtmlPath = Path.Combine(Path.GetTempPath(),
                                Path.GetFileNameWithoutExtension(msgFilePath) + ".mhtml");

                            mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);

                            Document wordDoc = new Document(tempMhtmlPath);
                            DocumentBuilder builder = new DocumentBuilder(wordDoc);

                            foreach (MapiAttachment attachment in mapMsg.Attachments)
                            {
                                string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                                string uniqueAttachmentPath = attachmentPath;
                                int duplicateCount = 1;
                                while (File.Exists(uniqueAttachmentPath))
                                {
                                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(attachment.FileName);
                                    string ext = Path.GetExtension(attachment.FileName);
                                    uniqueAttachmentPath = Path.Combine(attachmentsDirectory,
                                        $"{fileNameWithoutExt}_{duplicateCount}{ext}");
                                    duplicateCount++;
                                }

                                attachment.Save(uniqueAttachmentPath);

                                builder.MoveToDocumentEnd();
                                builder.InsertHyperlink(attachment.FileName, uniqueAttachmentPath, false);
                                builder.InsertParagraph();
                            }

                            string pdfFilePath = Path.Combine(outputDirectory,
                                Path.GetFileNameWithoutExtension(msgFilePath) + ".pdf");
                            Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                            wordDoc.Save(pdfFilePath, pdfOptions);

                            if (File.Exists(tempMhtmlPath))
                            {
                                File.Delete(tempMhtmlPath);
                            }
                        }
                    }
                    catch (Exception exFile)
                    {
                        Console.Error.WriteLine($"Failed to process '{msgFilePath}': {exFile.Message}");
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
