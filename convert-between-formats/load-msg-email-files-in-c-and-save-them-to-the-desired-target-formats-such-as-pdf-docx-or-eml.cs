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
            string inputMsgPath = "input.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file using MapiMessage
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Convert to MailMessage with default conversion options
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMessage = mapMsg.ToMailMessage(conversionOptions))
            {
                // Ensure output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save as PDF
                string pdfPath = Path.Combine(outputDir, "converted.pdf");
                mailMessage.Save(pdfPath);

                // Save as DOCX
                string docxPath = Path.Combine(outputDir, "converted.docx");
                mailMessage.Save(docxPath);

                // Save as EML (preserving original format)
                string emlPath = Path.Combine(outputDir, "converted.eml");
                mailMessage.Save(emlPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
