using System;
using System.IO;
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
            string oftPath = "template.oft";
            string mhtmlPath = "temp.mhtml";
            string pdfPath = "output.pdf";

            // Ensure the OFT file exists; create a minimal placeholder if missing
            if (!File.Exists(oftPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(oftPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    var placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Sample OFT",
                        "This is a placeholder OFT template.");
                    placeholder.SaveAsTemplate(oftPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OFT: {ex.Message}");
                    return;
                }
            }

            // Load the OFT template, convert to MailMessage, and save as MHTML
            try
            {
                using (MapiMessage oftMessage = MapiMessage.Load(oftPath))
                {
                    MailMessage mailMessage = oftMessage.ToMailMessage(new MailConversionOptions());
                    mailMessage.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing OFT file: {ex.Message}");
                return;
            }

            // Load the MHTML into Aspose.Words and export to PDF (flattening form fields)
            try
            {
                Document doc = new Document(mhtmlPath);
                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                // By default Aspose.Words flattens form fields when saving to PDF
                doc.Save(pdfPath, pdfOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting to PDF: {ex.Message}");
                return;
            }
            finally
            {
                // Clean up temporary MHTML file
                try
                {
                    if (File.Exists(mhtmlPath))
                    {
                        File.Delete(mhtmlPath);
                    }
                }
                catch
                {
                    // Suppress any cleanup errors
                }
            }

            Console.WriteLine($"PDF successfully created at: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
