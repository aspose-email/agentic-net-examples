using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mime;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        // Placeholder for Aspose license (if available)
        // var emailLicense = new Aspose.Email.License();
        // emailLicense.SetLicense("Aspose.Email.lic");
        // var wordsLicense = new Aspose.Words.License();
        // wordsLicense.SetLicense("Aspose.Words.lic");

        string inputFolder = "InputEml";
        string outputFolder = "OutputPdf";

        if (!Directory.Exists(inputFolder))
        {
            Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
            return;
        }

        try
        {
            Directory.CreateDirectory(outputFolder);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
            return;
        }

        string[] emlFiles;
        try
        {
            emlFiles = Directory.GetFiles(inputFolder, "*.eml");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating EML files: {ex.Message}");
            return;
        }

        if (emlFiles.Length == 0)
        {
            Console.WriteLine("No EML files found to process.");
            return;
        }

        foreach (string emlPath in emlFiles)
        {
            try
            {
                // Load the EML file with options to preserve attachments and embedded messages
                var loadOptions = new EmlLoadOptions
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                using (MailMessage mailMessage = MailMessage.Load(emlPath, loadOptions))
                {
                    // Convert the email to MHTML using Aspose.Email
                    using (var mhtmlStream = new MemoryStream())
                    {
                        var mhtmlSaveOptions = Aspose.Email.SaveOptions.DefaultMhtml;
                        mailMessage.Save(mhtmlStream, mhtmlSaveOptions);
                        mhtmlStream.Position = 0; // Reset stream position for reading

                        // Load MHTML into Aspose.Words Document
                        var wordsDoc = new Document(mhtmlStream);

                        // Prepare PDF output path
                        string pdfFileName = Path.GetFileNameWithoutExtension(emlPath) + ".pdf";
                        string pdfPath = Path.Combine(outputFolder, pdfFileName);

                        // Save as PDF using Aspose.Words
                        var pdfSaveOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        wordsDoc.Save(pdfPath, pdfSaveOptions);

                        Console.WriteLine($"Converted '{emlPath}' to '{pdfPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert '{emlPath}': {ex.Message}");
            }
        }
    }
}
