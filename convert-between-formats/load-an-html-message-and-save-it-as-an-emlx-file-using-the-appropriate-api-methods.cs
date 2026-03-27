using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.emlx";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Message saved as EMLX to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
