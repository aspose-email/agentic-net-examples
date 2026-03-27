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
            // Input MSG file path
            string inputMsgPath = "sample.msg";

            // Verify the input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Convert the MapiMessage to a MailMessage
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Save as EML (always supported)
                    string emlPath = "output.eml";
                    mailMessage.Save(emlPath, SaveOptions.DefaultEml);

                    // If PDF and DOCX formats are required and supported by the installed Aspose.Email version,
                    // they can be saved using the corresponding SaveOptions, e.g. SaveOptions.DefaultPdf and SaveOptions.DefaultDocx.
                    // Uncomment the following lines if those members are available:

                    // string pdfPath = "output.pdf";
                    // mailMessage.Save(pdfPath, SaveOptions.DefaultPdf);

                    // string docxPath = "output.docx";
                    // mailMessage.Save(docxPath, SaveOptions.DefaultDocx);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
