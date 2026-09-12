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
        const string inputOftPath = "template.oft";
        const string outputPdfPath = "output.pdf";

        // Ensure the input OFT file exists; create a simple placeholder if it does not.
        if (!File.Exists(inputOftPath))
        {
            try
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder Subject",
                    "Placeholder body."))
                {
                    placeholder.Save(inputOftPath);
                }
                Console.WriteLine($"Placeholder OFT created at '{inputOftPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating placeholder OFT: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the OFT template as a MAPI message
            MapiMessage oftMessage = MapiMessage.Load(inputOftPath);

            // Convert to MailMessage for MHTML export
            MailMessage mailMessage = oftMessage.ToMailMessage(new MailConversionOptions());

            // Export MailMessage to MHTML in memory
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;

                // Load MHTML into Aspose.Words Document
                Document doc = new Document(mhtmlStream);

                // Prepare PDF save options (no explicit form‑field flattening property to maintain compatibility)
                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the document as PDF
                doc.Save(outputPdfPath, pdfOptions);
            }

            Console.WriteLine($"Successfully converted '{inputOftPath}' to PDF.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}
