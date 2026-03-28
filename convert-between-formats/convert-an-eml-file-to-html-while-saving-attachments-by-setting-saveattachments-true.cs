using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                MhtSaveOptions saveOptions = new MhtSaveOptions()
                {
                    SaveAttachments = true
                };

                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
