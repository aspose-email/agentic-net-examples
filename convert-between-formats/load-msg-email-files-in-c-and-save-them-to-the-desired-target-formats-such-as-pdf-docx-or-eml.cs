using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMsg = MapiMessage.Load(inputMsgPath))
            {
                // Convert to MailMessage for formats supported by MailMessage.Save
                MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                // Save as EML
                string emlPath = Path.ChangeExtension(inputMsgPath, ".eml");
                mailMsg.Save(emlPath, SaveOptions.DefaultEml);
                Console.WriteLine($"Saved EML to {emlPath}");

                // The following formats (PDF, DOCX) are not directly supported via SaveOptions in this version.
                // If needed, they can be implemented using appropriate Aspose.Email conversion APIs
                // when such overloads become available.
                /*
                // Example placeholder for PDF conversion (requires a valid PdfSaveOptions class)
                // string pdfPath = Path.ChangeExtension(inputMsgPath, ".pdf");
                // var pdfOptions = new Aspose.Email.Pdf.PdfSaveOptions();
                // mailMsg.Save(pdfPath, pdfOptions);

                // Example placeholder for DOCX conversion (requires a valid DocxSaveOptions class)
                // string docxPath = Path.ChangeExtension(inputMsgPath, ".docx");
                // var docxOptions = new Aspose.Email.Docx.DocxSaveOptions();
                // mailMsg.Save(docxPath, docxOptions);
                */
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}