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
            string inputPath = "input.msg";
            string outputPath = "output.emlx";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                    mailMessage.Save(outputPath, saveOptions);
                }
            }

            Console.WriteLine($"Message converted and saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}