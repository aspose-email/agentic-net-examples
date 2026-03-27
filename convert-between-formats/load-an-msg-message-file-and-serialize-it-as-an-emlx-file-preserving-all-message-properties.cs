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
            string outputEmlxPath = "output.emlx";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputEmlxPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load MSG as MapiMessage and convert to MailMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Save as EMLX preserving all properties
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                    mailMessage.Save(outputEmlxPath, saveOptions);
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
